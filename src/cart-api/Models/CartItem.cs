namespace cart_api.Models
{
    public class CartItem
    {
        public string ProductId { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public int Quantity { get; set; }
    }
}