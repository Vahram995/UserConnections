using Users.DAL.Entities;

namespace Users.DAL.Repositories.Abstract
{
    public interface IUserConnectionRepository
    {
        Task AddAsync(UserConnection userConnection);
        Task<List<UserConnection>> GetByIpAddressAsync(string ipAddress);
        Task<List<UserConnection>> GetByUserIdAsync(long userId);
        Task<List<string>> GetUserIpAddressesAsync(long userId);
        Task<UserConnection> GetUserLastConnectionAsync(long userId);
        Task<List<long>> GetUsersByIpPrefixAsync(string ipPrefix);
        Task<Dictionary<string, DateTime>> GetLastSeenPerIpAsync(long userId);
        Task SaveChangesAsync();
    }
}