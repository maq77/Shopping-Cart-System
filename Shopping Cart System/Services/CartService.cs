using Shopping_Cart_System.Models.Requests;
using Shopping_Cart_System.Models.Responses;
using Shopping_Cart_System.Models;

namespace Shopping_Cart_System.Services
{
    public class CartService : ICartService
    {
        // In-memory store for user carts
        private static Dictionary<string, Cart> _carts = new Dictionary<string, Cart>();

        // Mock product catalog (in a real app, this would come from another service or database) | Seeding Data
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10.99 },
            new Product { Id = 2, Name = "Product 2", Price = 24.99 },
            new Product { Id = 3, Name = "Product 3", Price = 5.49 },
            new Product { Id = 4, Name = "Product 4", Price = 99.99 },
            new Product { Id = 5, Name = "Product 5", Price = 49.99 }
        };

        public Cart GetCart(string userId)
        {
            if (!_carts.ContainsKey(userId))
            {
                _carts[userId] = new Cart(userId); // Create a new cart if it doesn't exist and assigns every user a unique cart
            }

            return _carts[userId];
        }

        public Cart AddToCart(string userId, AddToCartRequest request)
        {
            // Validate product exists
            var product = _products.FirstOrDefault(p => p.Id == request.ProductId);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {request.ProductId} not found.");
            }

            // Validate quantity
            if (request.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            // Get or create user's cart
            var cart = GetCart(userId);

            // Check if item already exists in cart
            var existingItem = cart.cartItems.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                // Add new item
                cart.cartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = request.Quantity,
                    PriceAtTimeofAdd = product.Price
                });
            }

            return cart;
        }
        public Cart UpdateQuantity(string userId, UpdateCartItemRequest request)
        {
            var cart = GetCart(userId);
            var item = cart.cartItems.FirstOrDefault(i => i.ProductId == request.ProductId);
            // Validate item exists in cart
            if (item == null)
            {
                throw new ArgumentException($"Product with ID {request.ProductId} not found in cart.");
            }
            // Validate quantity
            if (request.Quantity < 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }
            else if (request.Quantity == 0)
            {
                // Remove item if quantity is zero
                cart.cartItems.Remove(item);
                return cart;
            }
            // Update quantity
            item.Quantity = request.Quantity;
            return cart;
        }

        public Cart RemoveFromCart(string userId, RemoveFromCartRequest request)
        {
            var cart = GetCart(userId);
            var item = cart.cartItems.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (item == null)
            {
                throw new ArgumentException($"Product with ID {request.ProductId} not found in cart.");
            }

            // If quantity is specified and less than item quantity, reduce quantity
            if (request.Quantity > 0 && request.Quantity < item.Quantity)
            {
                item.Quantity -= request.Quantity;
            }
            else
            {
                // Otherwise, remove the item completely
                cart.cartItems.Remove(item);
            }

            return cart;
        }

        public CheckoutResponse Checkout(string userId, CheckoutRequest request)
        {
            var cart = GetCart(userId);

            // Validate cart is not empty
            if (!cart.cartItems.Any())
            {
                throw new InvalidOperationException("Cannot checkout with an empty cart.");
            }

            // Create order ID (in a real app this would be stored in a database)
            var orderId = Guid.NewGuid().ToString("N");

            // Simulate successful payment processing
            var response = new CheckoutResponse
            {
                OrderId = orderId,
                Total = cart.Total,
                Items = new List<CartItem>(cart.cartItems), // Copy items for the receipt
                PaymentSuccessful = true,
                Message = "Payment processed successfully."
            };

            // Clear the cart
            ClearCart(userId);

            return response;
        }
        public Cart ClearCart(string userId)
        {
            var cart = GetCart(userId);
            cart.cartItems.Clear();
            return cart;
        }
        public void ClearCarts()
        {

            _carts.Clear(); // Clear all carts
        }
    }
}
