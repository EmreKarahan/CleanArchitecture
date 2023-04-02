using Application.Common.Caching;
using Enyim.Caching;

namespace Infrastructure.Caching;

public class MemcachedManager : ICachingManager
{
    private readonly IMemcachedClient _memcachedClient;
    private readonly object _lockObj = new object();

    public MemcachedManager(IMemcachedClient memcachedClient)
    {
        _memcachedClient = memcachedClient;
    }
    
    public async Task<bool> SetAsync<T>(string key, T value, int seconds)
    {
        return await _memcachedClient.SetAsync(key, value, seconds);
    }
    public async Task<bool> SetAsync<T>(string key, T value, uint seconds)
    {
        return await _memcachedClient.SetAsync(key, value, seconds);
    }
    
    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan timeSpan)
    {
        return await _memcachedClient.SetAsync(key, value, timeSpan);
    }
    
    public async Task<T> GetValueAsync<T>(string key)
    {
        var result = await _memcachedClient.GetValueAsync<T>(key);
        return result;
    }
    
    public async Task<T> GetValueOrCreateAsync<T>(string key, int cacheSeconds, Func<Task<T>> createFunc)
    {
        var result = await _memcachedClient.GetValueOrCreateAsync<T>(key, cacheSeconds, createFunc);
        return result;
    }
}