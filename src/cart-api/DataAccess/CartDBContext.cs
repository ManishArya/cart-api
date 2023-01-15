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

        public IMongoCollection<TDocument> Get<TDocument>(string name) => _database.GetCollection<TDocument>(name);
    }
}