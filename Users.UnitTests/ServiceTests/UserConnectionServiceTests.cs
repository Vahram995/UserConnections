using Microsoft.Extensions.Logging;
using Moq;
using Users.DAL.Entities;
using Users.DAL.Repositories.Abstract;
using Users.Services.Abstract;
using Users.Services.Concrete;

namespace Users.UnitTests.ServiceTests
{
    public class UserConnectionServiceTests
    {
        private readonly Mock<IUserConnectionRepository> _repo = new();
        private readonly Mock<IRedisCacheService> _cache = new();
        private readonly Mock<ILogger<UserConnectionService>> _log = new();
        private readonly UserConnectionService _service;

        public UserConnectionServiceTests()
            => _service = new UserConnectionService(_repo.Object, _cache.Object, _log.Object);

        [Fact]
        public async Task AddConnection_persists_and_caches()
        {
            // Act
            await _service.AddConnectionAsync(123, "127.0.0.1");

            // Assert
            _repo.Verify(r => r.AddAsync(It.Is<UserConnection>(u =>
                u.UserId == 123 && u.IpAddress == "127.0.0.1")), Times.Once);
            _repo.Verify(r => r.SaveChangesAsync(), Times.Once);

            // Assert 
            _cache.Verify(c => c.SetUserLastConnectionAsync(123, It.IsAny<DateTime>(), "127.0.0.1"), Times.Once);
            _cache.Verify(c => c.SetLastSeenByIpAsync("127.0.0.1", It.IsAny<DateTime>()), Times.Once);
        }
    }
}
