namespace cart_api.DataAccess
{
    public class CartStoreDBSettings : ICartStoreDBSetting
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}