namespace Users.Services.Abstract
{
    public interface IRedisCacheService
    {
        Task SetUserLastConnectionAsync(long userId, DateTime time, string ip);
        Task<(DateTime Time, string Ip)?> GetUserLastConnectionAsync(long userId);
        Task SetLastSeenByIpAsync(string ip, DateTime time);
        Task<DateTime?> GetLastSeenByIpAsync(string ip);
    }
}