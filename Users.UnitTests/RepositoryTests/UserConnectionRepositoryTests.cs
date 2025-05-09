using Microsoft.EntityFrameworkCore;
using Users.DAL.Entities;
using Users.DAL.Repositories.Concrete;
using Users.DAL;
using FluentAssertions;

namespace Users.UnitTests.RepositoryTests
{
    public class UserConnectionRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UserConnectionRepository _repository;

        public UserConnectionRepositoryTests()
        {
            var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(opts);
            _repository = new UserConnectionRepository(_context);
        }

        [Fact]
        public async Task GetUsersByIpPrefixAsync_returns_distinct_userIds()
        {
            _context.UserConnections.AddRange(
                new UserConnection { UserId = 1, IpAddress = "31.214.157.141", ConnectedAt = DateTime.UtcNow },
                new UserConnection { UserId = 2, IpAddress = "62.4.36.194", ConnectedAt = DateTime.UtcNow },
                new UserConnection { UserId = 1, IpAddress = "31.214.0.1", ConnectedAt = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            var result = await _repository.GetUsersByIpPrefixAsync("31.214");

            result.Should().BeEquivalentTo(new[] { 1 });
        }

        public void Dispose() => _context.Dispose();
    }
}
