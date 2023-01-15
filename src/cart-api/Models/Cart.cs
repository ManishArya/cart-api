using System.Collections.Generic;

namespace cart_api.Models
{
    public class Cart : BaseDocument
    {
        public Cart() => Items = new HashSet<CartItem>();

        public string UserId { get; set; }

        public ICollection<CartItem> Items { get; set; }
    }
}