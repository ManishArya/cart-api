namespace cart_api.Models.Dtos
{
    public class CartItemDto
    {

        public string ProductId { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public int Quantity { get; set; }
    }
}