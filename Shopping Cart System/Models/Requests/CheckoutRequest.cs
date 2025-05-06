namespace Shopping_Cart_System.Models.Requests
{
    public class CheckoutRequest
    {
        public string PaymentMethod { get; set; } = "Cash on Delivery"; // Just for simulation
        public string ShippingAddress { get; set; } = "CodeZilla SLR Corp."; // for simulation
    }
}
