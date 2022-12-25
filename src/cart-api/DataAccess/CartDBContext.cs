using MongoDB.Driver;

namespace cart_api.DataAccess
{
    public class CartDBContext : ICartDBContext
    {
        private readonly IMongoDatabase _database;

        public CartDBContext(ICartStoreDBSetting cartStoreDatabaseSettings)
        {
            var client = new MongoClient(cartStoreDatabaseSettings.ConnectionString);
            _database = client.GetDatabase(cartStoreDatabaseSettings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);
    }
}