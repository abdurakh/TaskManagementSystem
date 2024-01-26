namespace TaskManagement.Domain.Common.Caching;

public class CacheSettings
{
    public int AbsoluteExpirationTimeInSeconds { get; set; }

    public int SlidingExpirationTimeInSeconds { get; set; }
}
