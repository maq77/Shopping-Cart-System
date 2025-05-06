namespace Shopping_Cart_System.Models.Responses
{
    public class CheckoutResponse
    {
        public string OrderId { get; set; }
        public double Total { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public bool PaymentSuccessful { get; set; }
        public string Message { get; set; } = "Thanks For Purchasing!";
    }
}
