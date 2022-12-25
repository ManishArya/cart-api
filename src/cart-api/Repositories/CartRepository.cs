using cart_api.Models;
using cart_api.DataAccess;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;

namespace cart_api.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(ICartDBContext context) : base(context) { }

        public async Task<string> GetCartIdAsync(Expression<Func<Cart, bool>> filter)
        {
            var projectionDefinition = ProjectionDefinitionBuilder.Include(p => p.Id);
            return (await GetDocumentAsync(filter, projectionDefinition))?.Id;
        }

        public async Task<int> GetTotalQuantityAsync(Expression<Func<Cart, bool>> filter) => await _collection.AsQueryable()
            .Where(filter).SelectMany(s => s.Items).Select(s => s.Quantity).SumAsync(s => s);

        public async Task<bool> AddCartItemAsync(string username, CartItem item)
        {
            var update = UpdateDefinitionBuilder.Push(u => u.Items, item);
            return await UpdateDocumentAsync(f => f.Username == username, update);
        }

        public async Task<bool> RemoveCartItemAsync(string username, string productId)
        {
            var update = UpdateDefinitionBuilder.PullFilter(f => f.Items, filter => filter.ProductId == productId);
            return await UpdateDocumentAsync(f => f.Username == username, update);
        }
    }
}