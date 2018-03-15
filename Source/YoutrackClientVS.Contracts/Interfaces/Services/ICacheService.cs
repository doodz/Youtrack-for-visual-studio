using YouTrackClientVS.Contracts.Models;

namespace YouTrackClientVS.Contracts.Interfaces.Services
{
    public interface ICacheService
    {
        void Add(string key, object value);
        Result<T> Get<T>(string key) where T : class;
        void Delete(string key);
    }
}