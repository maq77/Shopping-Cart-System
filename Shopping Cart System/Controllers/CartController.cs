using Microsoft.AspNetCore.Mvc;
using Shopping_Cart_System.Models.Requests;
using Shopping_Cart_System.Models.Responses;
using Shopping_Cart_System.Models;
using Shopping_Cart_System.Services;

namespace ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        //  we'll use a header to simulate user identification - ready to use in real apps 
        private string GetUserId()
        {
            // In a real app, this would come from authenticated user claims (JWT token)
            if (Request.Headers.TryGetValue("X-User-Id", out var userId) && !string.IsNullOrEmpty(userId))
            {
                return userId;
            }

            // Default test user
            return "test-user-123";
        }

        [HttpGet]
        public ActionResult<Cart> GetCart()
        {
            try
            {
                var cart = _cartService.GetCart(GetUserId());
                return Ok(cart);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("add")]
        public ActionResult<Cart> AddToCart(AddToCartRequest request)
        {
            try
            {
                var cart = _cartService.AddToCart(GetUserId(), request);
                return Ok(cart);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        [HttpPost("Update Quantity")]
        public ActionResult<Cart> UpdateQuantity(UpdateCartItemRequest request)
        {
            try
            {
                var cart = _cartService.UpdateQuantity(GetUserId(), request);
                return Ok(cart);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("remove")] //By request
        public ActionResult<Cart> RemoveFromCart(RemoveFromCartRequest request)
        {
            try
            {
                var cart = _cartService.RemoveFromCart(GetUserId(), request);
                return Ok(cart);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{productId}")] //By productId
        public ActionResult<Cart> RemoveFromCartAlternative(int productId)
        {
            try
            {
                var request = new RemoveFromCartRequest { ProductId = productId };
                var cart = _cartService.RemoveFromCart(GetUserId(), request);
                return Ok(cart);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("checkout")] // simulate checkout
        public ActionResult<CheckoutResponse> Checkout(CheckoutRequest request)
        {
            try
            {
                var response = _cartService.Checkout(GetUserId(), request);
                return Ok(response);
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message }); //internal server error
            }
        }
    }
}