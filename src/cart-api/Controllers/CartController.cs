using Microsoft.AspNetCore.Mvc;
using cart_api.Services;
using cart_api.Models.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using cart_api.Filters;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace cart_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [LoggingFilter]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService, ILogger<CartController> logger) : base(logger) => _cartService = cartService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _cartService.GetAsync();
            return ToSendResponse(result);
        }

        [HttpPost, Route("add")]
        public async Task<IActionResult> AddItem(CartItemDto item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            await _cartService.AddItemAsync(item);
            return ToSendResponse();
        }

        [HttpPut, Route("updateqty")]
        public async Task<IActionResult> UpdateQuantity(string productId, int quantity)
        {
            if (productId == null)
            {
                return BadRequest();
            }

            var result = await _cartService.UpdateQuantityAsync(productId, quantity);
            return ToSendResponse(result);
        }

        [HttpDelete, Route("cartitem")]
        public async Task<IActionResult> RemoveCartItem(string productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }

            var result = await _cartService.RemoveCartItemAsync(productId);
            return ToSendResponse(result);
        }

        [HttpGet, Route("counts")]
        public async Task<IActionResult> GetTotalQuantity() => ToSendResponse(await _cartService.GetTotalQuantityAsync());

        [HttpDelete, Route("removecart")]
        public async Task<IActionResult> RemoveCart() => ToSendResponse(await _cartService.RemoveCartAsync());
    }
}