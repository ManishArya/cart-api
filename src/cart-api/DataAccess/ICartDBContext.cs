using MongoDB.Driver;

namespace cart_api.DataAccess
{
    public interface ICartDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}