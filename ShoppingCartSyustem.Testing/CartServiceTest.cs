using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping_Cart_System.Models;
using Shopping_Cart_System.Models.Requests;
using Shopping_Cart_System.Models.Responses;
using Shopping_Cart_System.Services;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Net.Mime.MediaTypeNames;

namespace ShoppingCartSyustem.Testing
{
    [TestClass]
    public class CartServiceTests
    {
        private ICartService _cartService;
        private readonly string _testUserId = "test-user";
        private readonly string _testUserId2 = "test-user-2";

        [TestInitialize]
        public void Setup()
        {
            _cartService = new CartService();
        }

        [TestMethod]
        public void GetCart_NewUser_ReturnsEmptyCart()
        {
            // Act
            var cart = _cartService.GetCart(_testUserId2);

            // Assert
            Assert.IsNotNull(cart);
            Assert.AreEqual(_testUserId2, cart.UserId);
            Assert.AreEqual(0, cart.cartItems.Count);
            Assert.AreEqual(0, cart.Total);
        }

        [TestMethod]
        public void AddToCart_ValidProduct_AddsToCart()
        {
            _cartService.ClearCarts();
            // Arrange
            var request = new AddToCartRequest { ProductId = 1, Quantity = 2 };

            //_cartService.ClearCart(_testUserId); // Ensure the cart is empty before adding
            // Act
            var cart = _cartService.AddToCart(_testUserId, request);

            // Assert
            Assert.AreEqual(1, cart.cartItems.Count);
            var item = cart.cartItems.First();
            Assert.AreEqual(1, item.ProductId);
            Assert.AreEqual(2, item.Quantity);
            Assert.IsTrue(cart.Total > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddToCart_InvalidProduct_ThrowsException()
        {
            // Arrange
            var request = new AddToCartRequest { ProductId = 999, Quantity = 1 };

            // Act
            _cartService.AddToCart(_testUserId, request);

            // Assert: Exception expected
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddToCart_ZeroQuantity_ThrowsException()
        {
            // Arrange
            var request = new AddToCartRequest { ProductId = 1, Quantity = 0 };

            // Act
            _cartService.AddToCart(_testUserId, request);

            // Assert: Exception expected
        }

        [TestMethod]
        public void AddToCart_ExistingItem_IncreasesQuantity()
        {
            // Arrange
            var request1 = new AddToCartRequest { ProductId = 1, Quantity = 2 };
            var request2 = new AddToCartRequest { ProductId = 1, Quantity = 3 };

            // Act
            _cartService.ClearCarts();
            _cartService.AddToCart(_testUserId, request1);
            var cart = _cartService.AddToCart(_testUserId, request2);  // Ensure the cart gets updated

            // Assert
            Assert.AreEqual(1, cart.cartItems.Count);  // Only one item in the cart
            var item = cart.cartItems.First();
            Assert.AreEqual(1, item.ProductId);  // Ensure it's the correct product
            Assert.AreEqual(5, item.Quantity);  // Total quantity: 2 + 3
        }


        [TestMethod]
        public void RemoveFromCart_ExistingItem_DecreasesQuantity()
        {
            _cartService.ClearCarts();
            // Arrange
            _cartService.AddToCart(_testUserId, new AddToCartRequest { ProductId = 1, Quantity = 5 });
            var removeRequest = new RemoveFromCartRequest { ProductId = 1, Quantity = 2 };

            // Act
            var cart = _cartService.RemoveFromCart(_testUserId, removeRequest);

            // Assert
            Assert.AreEqual(1, cart.cartItems.Count);
            var item = cart.cartItems.First();
            Assert.AreEqual(3, item.Quantity); // 5 - 2
        }

        [TestMethod]
        public void RemoveFromCart_AllQuantity_RemovesItem()
        {
            _cartService.ClearCarts();
            // Arrange
            _cartService
                .AddToCart(_testUserId, new AddToCartRequest { ProductId = 1, Quantity = 2 });
            _cartService
                .AddToCart(_testUserId, new AddToCartRequest { ProductId = 2, Quantity = 3 });
            var removeRequest = new RemoveFromCartRequest { ProductId = 1, Quantity = 5 }; // More than current quantity

            // Act
            var cart = _cartService.RemoveFromCart(_testUserId, removeRequest);

            // Assert
            Assert.AreEqual(1, cart.cartItems.Count);
            Assert.AreEqual(2, cart.cartItems.First().ProductId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveFromCart_NonExistingItem_ThrowsException()
        {
            // Arrange
            var removeRequest = new RemoveFromCartRequest { ProductId = 999 };

            // Act
            _cartService.RemoveFromCart(_testUserId, removeRequest);

            // Assert: Exception expected
        }

        [TestMethod]
        public void Checkout_ValidCart_ReturnsSuccessfulResponse()
        {
            _cartService.ClearCarts();
            // Arrange
            _cartService.AddToCart(_testUserId, new AddToCartRequest { ProductId = 1, Quantity = 2 });
            var checkoutRequest = new CheckoutRequest();

            // Act
            var response = _cartService.Checkout(_testUserId, checkoutRequest);

            // Assert
            Assert.IsTrue(response.PaymentSuccessful);
            Assert.IsFalse(string.IsNullOrEmpty(response.OrderId));
            Assert.AreEqual(1, response.Items.Count);

            // Verify cart is cleared
            var cart = _cartService.GetCart(_testUserId);
            Assert.AreEqual(0, cart.cartItems.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Checkout_EmptyCart_ThrowsException()
        {
            // Arrange
            var checkoutRequest = new CheckoutRequest();

            // Act
            _cartService.Checkout(_testUserId, checkoutRequest);

            // Assert: Exception expected
        }
        [TestCleanup]
        public void Cleanup()
        {
            // Reset cart or clear global state after each test
            _cartService.ClearCarts(); // Clear the carts after the test execution
        }

    }
}
