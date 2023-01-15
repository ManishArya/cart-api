using cart_api.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace cart_api.Repositories
{
    public interface ICartRepository : IBaseRepository<Cart>
    {

        Task<string> GetCartIdAsync(Expression<Func<Cart, bool>> filter);

        Task<int> GetTotalQuantityAsync(Expression<Func<Cart, bool>> filter);

        Task<bool> AddCartItemAsync(string userId, CartItem item);

        Task<bool> RemoveCartItemAsync(string userId, string productId);
    }
}