using System;
using Microsoft.Extensions.Caching.Memory;

public static class CacheHelper
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

    // Long-term cache function (CachedLong): no expiration
    public static T GetOrSetLongCache<T>(string key, Func<T> createItem)
    {
        if (!_cache.TryGetValue(key, out T cacheEntry))
        {
            cacheEntry = createItem();
            _cache.Set(key, cacheEntry);  // No expiration
        }
        return cacheEntry;
    }

    // Short-term cache function (Cached): expires after 5 minutes
    public static T GetOrSetShortCache<T>(string key, Func<T> createItem)
    {
        if (!_cache.TryGetValue(key, out T cacheEntry))
        {
            cacheEntry = createItem();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));  // Cache for 5 minutes
            _cache.Set(key, cacheEntry, cacheEntryOptions);
        }
        return cacheEntry;
    }
}
