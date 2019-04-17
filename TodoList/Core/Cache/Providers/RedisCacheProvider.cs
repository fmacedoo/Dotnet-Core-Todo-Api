using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace TodoList.Core.Cache.Providers
{
    public class RedisCacheProvider : ICacheProvider
    {
        ConnectionMultiplexer connection;
        IDatabase database;

        public RedisCacheProvider(IConfiguration configuration)
        {
            var endpoint = configuration["Cache:RedisCacheProvider:Endpoint"];
            connection = ConnectionMultiplexer.Connect(endpoint);
            database = connection.GetDatabase();
        }

        public void Delete<T>(string key)
        {
            database.KeyDelete(key);
        }

        public T Get<T>(string key)
        {
            var value = database.StringGet(key);
            return !value.IsNull ? (T)Newtonsoft.Json.JsonConvert.DeserializeObject(value.ToString(), typeof(T)) : default(T);
        }

        public bool HasKey<T>(string key)
        {
            return database.KeyExists(key);
        }

        public void Store<T>(string key, T data)
        {
            var value = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            database.StringSet(key, value);
        }
    }
}