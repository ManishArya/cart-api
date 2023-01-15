using MongoDB.Driver;

namespace cart_api.DataAccess
{
    public interface ICartDBContext
    {
        IMongoCollection<TDocument> Get<TDocument>(string name);
    }
}