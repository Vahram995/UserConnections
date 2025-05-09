using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Users.DAL.Entities;

namespace Users.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserConnection> UserConnections { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}