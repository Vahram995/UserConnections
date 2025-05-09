using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Users.Services.Abstract;

namespace Users.Services.Concrete
{
    public class RedisCacheService(IDistributedCache cache) : IRedisCacheService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        public async Task SetUserLastConnectionAsync(long userId, DateTime time, string ip)
        {
            var data = JsonSerializer.Serialize(new LastConnection { Time = time, Ip = ip }, _jsonOptions);
            await cache.SetStringAsync($"user:last:{userId}", data, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });
        }

        public async Task<LastConnection> GetUserLastConnectionAsync(long userId)
        {
            var data = await cache.GetStringAsync($"user:last:{userId}");
            return data == null ? null : JsonSerializer.Deserialize<LastConnection>(data, _jsonOptions);
        }

        public async Task SetLastSeenByIpAsync(string ip, DateTime time)
        {
            await cache.SetStringAsync($"ip:last:{ip}", time.ToString(), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) });
        }

        public async Task<DateTime?> GetLastSeenByIpAsync(string ip)
        {
            var str = await cache.GetStringAsync($"ip:last:{ip}");
            return str == null ? null : DateTime.Parse(str);
        }
    }

    public class LastConnection
    {
        public DateTime Time { get; set; }
        public string Ip { get; set; }
    }
}
