namespace Users.Services.Abstract
{
    public interface IUserConnectionService
    {
        Task AddConnectionAsync(long userId, string ipAddress);
        Task<List<long>> FindUsersByIpPrefixAsync(string ipPrefix);
        Task<List<string>> GetUserIpsAsync(long userId);
        Task<(DateTime Time, string Ip)> GetUserLastConnectionAsync(long userId);
        Task<Dictionary<string, DateTime>> GetLastSeenPerIpAsync(long userId);
    }
}