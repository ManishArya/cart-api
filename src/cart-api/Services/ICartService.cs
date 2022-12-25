using cart_api.Models.Dtos;
using cart_api.Models;
using System.Threading.Tasks;

namespace cart_api.Services
{
    public interface ICartService
    {
        Task<ApiResponse<CartDto>> GetAsync();

        Task AddItemAsync(CartItemDto cart);

        Task<ApiResponse<bool>> UpdateQuantityAsync(string productId, int quantity);

        Task<ApiResponse<bool>> RemoveCartItemAsync(string productId);

        Task<BaseResponse> RemoveCartAsync();

        Task<ApiResponse<int>> GetTotalQuantityAsync();
    }
}