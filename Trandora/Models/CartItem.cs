namespace Trandora.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }    
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }
}
