using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Users.DAL.Entities;

namespace Users.DAL.EntityConfigurations
{
    internal class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
    {
        public void Configure(EntityTypeBuilder<UserConnection> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.IpAddress);
            builder.HasIndex(e => new { e.UserId, e.ConnectedAt });
            builder.HasIndex(e => new { e.IpAddress, e.ConnectedAt });

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserConnections);
        }
    }
}