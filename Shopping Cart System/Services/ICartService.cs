using Shopping_Cart_System.Models.Requests;
using Shopping_Cart_System.Models.Responses;
using Shopping_Cart_System.Models;

namespace Shopping_Cart_System.Services
{
    public interface ICartService
    {
        Cart GetCart(string userId);
        Cart AddToCart(string userId, AddToCartRequest request);
        Cart UpdateQuantity(string userId, UpdateCartItemRequest request);
        Cart RemoveFromCart(string userId, RemoveFromCartRequest request);
        CheckoutResponse Checkout(string userId, CheckoutRequest request);
        Cart ClearCart(string userId);
        void ClearCarts();
    }
}
