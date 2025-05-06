namespace Shopping_Cart_System.Models
{
    public class CartItem
    {
        public int ProductId { get; set; } //could be navigation property or FK
        public string ProductName { get; set; }
        public double PriceAtTimeofAdd { get; set; }
        public int Quantity { get; set; }
        public double Subtotal => TotalPrice();
        /*public CartItem(int productId, string productName, double price, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            PriceAtTimeofAdd = price;
        }*/
        public double TotalPrice()
        {
            return PriceAtTimeofAdd * Quantity;
        }
    }
}
