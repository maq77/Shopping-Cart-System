namespace Shopping_Cart_System.Models
{
    public class Cart
    {
        public string UserId { get; set; }
        public List<CartItem> cartItems = new List<CartItem>();
        public double Total => CalculateTotal();
        public Cart() { } // Default constructor 
        public Cart(string userId) // Constructor with userId
        {
            UserId = userId;
        }
        private double CalculateTotal()
        {
            double total = 0;
            foreach (var item in cartItems)
            {
                total += item.Subtotal;
            }
            return total;
        }

    }
}
