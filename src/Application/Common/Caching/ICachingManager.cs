namespace Application.Common.Caching;

public interface ICachingManager
{
    Task<bool> SetAsync<T>(string key, T value, int seconds);
    Task<bool> SetAsync<T>(string key, T value, uint seconds);
    Task<bool> SetAsync<T>(string key, T value, TimeSpan timeSpan);
    Task<T> GetValueAsync<T>(string key);
    Task<T> GetValueOrCreateAsync<T>(string key, int cacheSeconds, Func<Task<T>> createFunc);

}