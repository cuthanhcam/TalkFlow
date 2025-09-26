using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room;
namespace TalkFlow.Infrastructure.Data.Configurations
{
    public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.HasKey(c => c.ConnectionId);

            builder.Property(c => c.ConnectionId)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(c => c.UserId)
                .HasConversion(id => id.Value, value => new Domain.ValueObjects.User.UserId(value))
                .IsRequired();

            builder.Property(c => c.RoomId)
                .IsRequired();

            builder.Property(c => c.ConnectedAt)
                .IsRequired();

            builder.HasIndex(c => c.UserId);
            builder.HasIndex(c => c.ConnectedAt);
        }
    }
}
