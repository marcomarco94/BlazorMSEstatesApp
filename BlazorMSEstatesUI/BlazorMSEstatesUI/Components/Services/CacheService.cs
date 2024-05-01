using Microsoft.Extensions.Caching.Memory;

namespace BlazorMSEstatesUI.Components.Services;

public class CacheService
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T Get<T>(string key)
    {
        _cache.TryGetValue(key, out T value);
        return value;
    }

    public void Set<T>(string key, T value)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(_cacheDuration);
        _cache.Set(key, value, cacheEntryOptions);
    }
}