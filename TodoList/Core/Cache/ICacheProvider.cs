namespace TodoList.Core.Cache
{
    public interface ICacheProvider
    {
        void Delete<T>(string key);
        T Get<T>(string key);
        bool HasKey<T>(string key);
        void Store<T>(string key, T data);
    }
}