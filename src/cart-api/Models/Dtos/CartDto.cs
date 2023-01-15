using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace cart_api.Models.Dtos
{
    public class CartDto
    {
        public CartDto()
        {
            Items = new HashSet<CartItemDto>();
        }

        public string Id { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }

        public IEnumerable<CartItemDto> Items { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(c => c.Price * c.Quantity);
            }
        }

        public int TotalQuantity
        {
            get
            {
                return Items.Sum(c => c.Quantity);
            }
        }
    }
}