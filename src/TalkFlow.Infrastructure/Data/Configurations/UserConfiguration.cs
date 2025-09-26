using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.User;

namespace TalkFlow.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasConversion(id => id, id => id);

            builder.Property(u => u.DisplayName)
                .HasConversion(name => name.Value, value => new Domain.ValueObjects.User.DisplayName(value))
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.PhotoUrl)
                .HasConversion(url => url.Value, value => new Domain.ValueObjects.User.PhotoUrl(value))
                .HasMaxLength(500);

            builder.Property(u => u.Gender)
                .HasConversion(gender => gender != null ? gender.Value : null,
                              value => !string.IsNullOrEmpty(value) ? new Domain.ValueObjects.User.Gender(value) : null)
                .HasMaxLength(20);

            builder.Property(u => u.Age)
                .HasConversion(age => age != null ? age.Value : (int?)null,
                              value => value.HasValue ? new Domain.ValueObjects.User.Age(value.Value) : null);

            builder.Property(u => u.Nationality)
                .HasConversion(nationality => nationality != null ? nationality.Value : null,
                              value => !string.IsNullOrEmpty(value) ? new Domain.ValueObjects.User.Nationality(value) : null)
                .HasMaxLength(100);

            builder.Property(u => u.LastActive)
                .IsRequired();

            builder.Property(u => u.IsLocked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(u => u.Profile)
                .WithOne()
                .HasForeignKey<UserProfile>("UserId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.StrangerFilter)
                .WithOne()
                .HasForeignKey<StrangerFilter>("UserId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.DisplayName).IsUnique();
            builder.HasIndex(u => u.Email);
            builder.HasIndex(u => u.LastActive);
        }
    }
}
