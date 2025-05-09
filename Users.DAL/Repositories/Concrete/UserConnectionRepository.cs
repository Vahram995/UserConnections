using Microsoft.EntityFrameworkCore;
using Users.DAL.Entities;
using Users.DAL.Repositories.Abstract;

namespace Users.DAL.Repositories.Concrete
{
    public class UserConnectionRepository(ApplicationDbContext dbContext) : IUserConnectionRepository
    {
        public async Task AddAsync(UserConnection userConnection)
        {
            await dbContext.AddAsync(userConnection);
        }

        public async Task<List<UserConnection>> GetByIpAddressAsync(string ipAddress)
        {
            return await dbContext.UserConnections
                .Where(x => x.IpAddress == ipAddress)
                .ToListAsync();
        }

        public async Task<List<UserConnection>> GetByUserIdAsync(long userId)
        {
            return await dbContext.UserConnections
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<Dictionary<string, DateTime>> GetLastSeenPerIpAsync(long userId)
        {
            return await dbContext.UserConnections
                .Where(e => e.UserId == userId)
                .GroupBy(e => e.IpAddress)
                .Select(g => new { g.Key, LastSeen = g.Max(x => x.ConnectedAt) })
                .ToDictionaryAsync(x => x.Key, x => x.LastSeen);
        }

        public async Task<List<string>> GetUserIpAddressesAsync(long userId)
        {
            return await dbContext.UserConnections
                .Where(x => x.UserId == userId)
                .Select(x => x.IpAddress)
                .Distinct()
                .ToListAsync();
        }

        public async Task<UserConnection> GetUserLastConnectionAsync(long userId)
        {
            return await dbContext.UserConnections
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ConnectedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<long>> GetUsersByIpPrefixAsync(string ipPrefix)
        {
            return await dbContext.UserConnections
                .Where(x => x.IpAddress.StartsWith(ipPrefix))
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}