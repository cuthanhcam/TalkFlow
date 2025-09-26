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
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(up => up.Id);

            builder.Property(up => up.Id)
                .IsRequired();

            builder.Property(up => up.UserId)
                .IsRequired();

            builder.Property(up => up.Gender)
                .HasConversion(gender => gender != null ? gender.Value : null,
                              value => !string.IsNullOrEmpty(value) ? new Domain.ValueObjects.User.Gender(value) : null)
                .HasMaxLength(20);

            builder.Property(up => up.Age)
                .HasConversion(age => age != null ? age.Value : (int?)null,
                              value => value.HasValue ? new Domain.ValueObjects.User.Age(value.Value) : null);

            builder.Property(up => up.Nationality)
                .HasConversion(nationality => nationality != null ? nationality.Value : null,
                              value => !string.IsNullOrEmpty(value) ? new Domain.ValueObjects.User.Nationality(value) : null)
                .HasMaxLength(100);

            builder.Property(up => up.PhotoUrl)
                .HasConversion(url => url.Value, value => new Domain.ValueObjects.User.PhotoUrl(value))
                .HasMaxLength(500);

            builder.Property(up => up.CreatedAt)
                .IsRequired();

            builder.Property(up => up.UpdatedAt);

            builder.HasIndex(up => up.UserId).IsUnique();
            builder.HasIndex(up => up.CreatedAt);
        }
    }
}
