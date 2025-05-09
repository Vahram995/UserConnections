using Users.Services.Concrete;

namespace Users.Services.Abstract
{
    public interface IUserConnectionService
    {
        Task AddConnectionAsync(long userId, string ipAddress);
        Task<List<long>> FindUsersByIpPrefixAsync(string ipPrefix);
        Task<List<string>> GetUserIpsAsync(long userId);
        Task<LastConnection> GetUserLastConnectionAsync(long userId);
        Task<Dictionary<string, DateTime>> GetLastSeenPerIpAsync(long userId);
    }
}