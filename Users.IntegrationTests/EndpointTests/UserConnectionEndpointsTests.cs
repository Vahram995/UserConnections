using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Users.Services.Concrete;

namespace Users.IntegrationTests.Endpoints;

public class UserConnectionEndpointsMoreTests : IClassFixture<AppFactory>
{
    private readonly HttpClient _client;

    public UserConnectionEndpointsMoreTests(AppFactory factory)
        => _client = factory.CreateClient();

    private async Task AddAsync(long userId, string ip)
        => await _client.PostAsJsonAsync("/api/UserConnection", new { userId, ipAddress = ip });

    [Fact]
    public async Task GetIpsByUser_returns_distinct_ips()
    {
        await AddAsync(7, "10.0.0.1");
        await AddAsync(7, "10.0.0.2");
        await AddAsync(7, "10.0.0.1");   
        await AddAsync(8, "192.168.0.1");

        var ips = await _client.GetFromJsonAsync<string[]>("/api/UserConnection/7/ips");

        ips.Should().BeEquivalentTo(new[] { "10.0.0.1", "10.0.0.2" });
    }

    [Fact]
    public async Task GetLastConnectionByUser_returns_most_recent_entry()
    {
        await AddAsync(15, "1.1.1.1");
        await Task.Delay(20);            
        await AddAsync(15, "8.8.8.8");

        var last = await _client.GetFromJsonAsync<LastConnection>("/api/UserConnection/15/last");

        last.Should().NotBeNull();
        last!.Ip.Should().Be("8.8.8.8");
        last.Time.Should().BeOnOrAfter(DateTime.UtcNow.AddSeconds(-1));
    }

    [Fact]
    public async Task SearchByIp_handles_ipv6()
    {
        await AddAsync(55, "2001:db8::1");
        await AddAsync(66, "2001:db8:abcd::1");
        await AddAsync(77, "fe80::1");

        var users = await _client.GetFromJsonAsync<long[]>("/api/UserConnection/searchByIp?ipPrefix=2001:db8");

        users.Should().BeEquivalentTo(new[] { 55L, 66L });
    }
}