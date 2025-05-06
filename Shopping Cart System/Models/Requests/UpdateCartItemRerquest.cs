namespace Shopping_Cart_System.Models.Requests
{
    public class UpdateCartItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1; // Default to 1 if not specified
    }
}
