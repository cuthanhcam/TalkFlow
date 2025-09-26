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
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .IsRequired();

            builder.Property(r => r.Name)
                .HasConversion(name => name.Value, value => new Domain.ValueObjects.Room.RoomName(value))
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(r => r.SecurityCode)
                .HasConversion(code => code.Value, value => new Domain.ValueObjects.Room.SecurityCode(value))
                .HasMaxLength(20);

            builder.Property(r => r.Capacity)
                .HasConversion(capacity => capacity.Value, value => new Domain.ValueObjects.Room.RoomCapacity(value))
                .IsRequired();

            builder.Property(r => r.HostId)
                .HasConversion(id => id.Value, value => new Domain.ValueObjects.User.UserId(value))
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.IsChatBlocked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasMany(r => r.Connections)
                .WithOne()
                .HasForeignKey(c => c.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Messages)
                .WithOne()
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => r.Name).IsUnique();
            builder.HasIndex(r => r.SecurityCode);
            builder.HasIndex(r => r.HostId);
            builder.HasIndex(r => r.CreatedAt);
            builder.HasIndex(r => r.IsActive);
        }
    }
}
