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
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .IsRequired();

            builder.Property(m => m.RoomId)
                .IsRequired();

            builder.Property(m => m.SenderId)
                .HasConversion(id => id.Value, value => new Domain.ValueObjects.User.UserId(value))
                .IsRequired();

            builder.Property(m => m.SenderDisplayName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.Content)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(m => m.SentAt)
                .IsRequired();

            builder.Property(m => m.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasIndex(m => m.SenderId);
            builder.HasIndex(m => m.SentAt);
            builder.HasIndex(m => m.IsDeleted);
        }
    }
}
