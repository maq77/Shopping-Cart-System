namespace Shopping_Cart_System.Models.Requests
{
    public class RemoveFromCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } // If not specified, or equals item quantity or quals 0 , removes the entire item
    }
}
