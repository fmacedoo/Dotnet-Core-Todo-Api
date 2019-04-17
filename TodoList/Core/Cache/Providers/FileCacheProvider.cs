using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace TodoList.Core.Cache.Providers
{
    public class FileCacheProvider : ICacheProvider
    {
        readonly string FILE_NAME;

        public FileCacheProvider(IConfiguration configuration)
        {
            FILE_NAME = configuration["Cache:FileCacheProvider:File"];
            
            if (!File.Exists(FILE_NAME))
            {
                File.AppendAllText(FILE_NAME, "[]");
            }
        }

        private List<Item<T>> readFile<T>()
        {
            var text = File.ReadAllText(FILE_NAME);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Item<T>>>(text);
        }

        private void writeFile<T>(List<Item<T>> list)
        {
            var text = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            File.WriteAllText(FILE_NAME, text);
        }

        public void Delete<T>(string key)
        {
            var list = readFile<T>();
            list.RemoveAll(o => o.Key == key);
            writeFile(list);
        }

        public T Get<T>(string key)
        {
            var list = readFile<T>();
            var item = list.SingleOrDefault(o => o.Key == key);
            return item != null ? item.Value : default(T);
        }

        public bool HasKey<T>(string key)
        {
            var list = readFile<T>();
            return list.Any(o => o.Key == key);
        }

        public void Store<T>(string key, T data)
        {
            var list = readFile<T>();
            list.Add(new Item<T> { Key = key, Value = data });
            writeFile(list);
        }
    }

    public class Item<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }
}