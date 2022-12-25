using cart_api.Repositories;
using cart_api.Models;
using cart_api.Models.Dtos;
using AutoMapper;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace cart_api.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        private string Username => _contextAccessor.HttpContext.User.Identity.Name;

        public async Task<ApiResponse<CartDto>> GetAsync()
        {
            var cartEntity = await _cartRepository.GetDocumentAsync(f => f.Username == Username);
            var cart = _mapper.Map<CartDto>(cartEntity);
            return new ApiResponse<CartDto>(cart);
        }

        public async Task AddItemAsync(CartItemDto cartItemDto)
        {
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            var cartId = await _cartRepository.GetCartIdAsync(d => d.Username == Username);

            if (cartId == null)
            {
                var cart = new Cart()
                {
                    Username = Username,
                };
                cart.Items.Add(cartItem);
                await _cartRepository.AddDocumentAsync(cart);
            }
            else
            {

                var cartDocument = await _cartRepository.GetDocumentAsync(d => d.Id == cartId && d.Items.Any(item => item.ProductId == cartItemDto.ProductId));

                if (cartDocument == null)
                {
                    cartItem.Quantity = 1;
                    await _cartRepository.AddCartItemAsync(Username, cartItem);
                }
                else
                {
                    var quantity = cartDocument.Items.First().Quantity + 1;
                    await UpdateQuantityAsync(cartItem.ProductId, quantity);
                }
            }
        }

        public async Task<ApiResponse<bool>> UpdateQuantityAsync(string productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new InvalidOperationException("Negative or zero quantity can not be updated");
            }

            Expression<Func<Cart, bool>> filter = f => f.Username == Username && f.Items.Any(item => item.ProductId == productId);
            Expression<Func<Cart, int>> field = f => f.Items.ElementAt(-1).Quantity;

            var result = await _cartRepository.UpdateDocumentAsync(filter, field, quantity);
            return new ApiResponse<bool>(result);
        }

        public async Task<ApiResponse<bool>> RemoveCartItemAsync(string productId)
        {
            var result = await _cartRepository.RemoveCartItemAsync(Username, productId);
            return new ApiResponse<bool>(result);
        }

        public async Task<BaseResponse> RemoveCartAsync()
        {
            await _cartRepository.RemoveDocumentAsync(c => c.Username == Username);
            return new BaseResponse(System.Net.HttpStatusCode.OK);
        }

        public async Task<ApiResponse<int>> GetTotalQuantityAsync()
        {
            var count = await _cartRepository.GetTotalQuantityAsync(c => c.Username == Username);
            return new ApiResponse<int>(count);
        }
    }
}