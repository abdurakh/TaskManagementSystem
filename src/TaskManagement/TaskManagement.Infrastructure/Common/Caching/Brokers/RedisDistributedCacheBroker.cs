using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TaskManagement.Domain.Common.Caching;
using TaskManagement.Persistence.Caching.Brokers;

namespace TaskManagement.Infrastructure.Common.Caching.Brokers;

public class RedisDistributedCacheBroker : ICacheBroker
{
    private readonly CacheSettings _cacheSettings;
    private readonly IDistributedCache _distributedCache;
    private DistributedCacheEntryOptions _cacheEntryOptions;

    public RedisDistributedCacheBroker(
        IOptions<CacheSettings> cacheSettings,
        IDistributedCache distributedCache)
    {
        _cacheSettings = cacheSettings.Value;
        _distributedCache = distributedCache;

        _cacheEntryOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheSettings.AbsoluteExpirationTimeInSeconds),
            SlidingExpiration = TimeSpan.FromSeconds(_cacheSettings.SlidingExpirationTimeInSeconds)
        };
    }

    public async ValueTask<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
        return cachedValue is not null ? JsonSerializer.Deserialize<T>(cachedValue) : default;
    }

    public async ValueTask<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (cachedValue is not null) return JsonSerializer.Deserialize<T>(cachedValue);

        var value = await valueFactory();
        await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), _cacheEntryOptions, cancellationToken);
        return value;
    }

    public async ValueTask SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        => await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), _cacheEntryOptions, cancellationToken);

    public async ValueTask DeleteAsync(string key, CancellationToken cancellationToken = default)
        => await _distributedCache.RemoveAsync(key, cancellationToken);
}
