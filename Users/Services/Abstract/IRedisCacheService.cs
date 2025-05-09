using Users.Services.Concrete;

namespace Users.Services.Abstract
{
    public interface IRedisCacheService
    {
        Task SetUserLastConnectionAsync(long userId, DateTime time, string ip);
        Task<LastConnection> GetUserLastConnectionAsync(long userId);
        Task SetLastSeenByIpAsync(string ip, DateTime time);
        Task<DateTime?> GetLastSeenByIpAsync(string ip);
    }
}