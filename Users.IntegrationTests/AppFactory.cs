using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Users.DAL;

namespace Users.IntegrationTests
{
    public class AppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbDesc = services.Single(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                services.Remove(dbDesc);

                services.AddDbContextPool<ApplicationDbContext>(opt =>
                    opt.UseInMemoryDatabase("TestDb"));

                services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
            });
        }
    }
}
