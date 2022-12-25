namespace cart_api.DataAccess
{
    public interface ICartStoreDBSetting
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}