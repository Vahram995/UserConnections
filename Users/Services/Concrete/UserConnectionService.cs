using Users.DAL.Entities;
using Users.DAL.Repositories.Abstract;
using Users.Services.Abstract;

namespace Users.Services.Concrete
{
    public class UserConnectionService(IUserConnectionRepository repository, IRedisCacheService cache, ILogger<UserConnectionService> logger) : IUserConnectionService
    {
        public async Task AddConnectionAsync(long userId, string ipAddress)
        {
            var now = DateTime.UtcNow;
            var userConnection = new UserConnection
            {
                UserId = userId,
                IpAddress = ipAddress,
                ConnectedAt = now
            };

            await repository.AddAsync(userConnection);
            await repository.SaveChangesAsync();

            await cache.SetUserLastConnectionAsync(userId, now, ipAddress);
            await cache.SetLastSeenByIpAsync(ipAddress, now);
        }

        public async Task<List<long>> FindUsersByIpPrefixAsync(string ipPrefix)
        {
            return await repository.GetUsersByIpPrefixAsync(ipPrefix);
        }

        public async Task<List<string>> GetUserIpsAsync(long userId)
        {
            return await repository.GetUserIpAddressesAsync(userId);
        }

        public async Task<(DateTime Time, string Ip)> GetUserLastConnectionAsync(long userId)
        {
            var lastConnection = await cache.GetUserLastConnectionAsync(userId);
            if (lastConnection != null) 
                return lastConnection.Value;

            var last = await repository.GetUserLastConnectionAsync(userId);

            await cache.SetUserLastConnectionAsync(userId, last.ConnectedAt, last.IpAddress);
            return (last.ConnectedAt, last.IpAddress);
        }

        public async Task<Dictionary<string, DateTime>> GetLastSeenPerIpAsync(long userId)
        {
            var lastSeens = await repository.GetLastSeenPerIpAsync(userId);

            return lastSeens;
        }
    }
}
