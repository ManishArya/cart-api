using AutoMapper;
using cart_api.Models;
using cart_api.Models.Dtos;

namespace cart_api.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartDto, Cart>();
            CreateMap<Cart, CartDto>();
            CreateMap<CartItemDto, CartItem>();
            CreateMap<CartItem, CartItemDto>();
        }
    }
}