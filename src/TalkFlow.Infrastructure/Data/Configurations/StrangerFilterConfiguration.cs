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
    public class StrangerFilterConfiguration : IEntityTypeConfiguration<StrangerFilter>
    {
        public void Configure(EntityTypeBuilder<StrangerFilter> builder)
        {
            builder.HasKey(sf => sf.Id);

            builder.Property(sf => sf.Id)
                .IsRequired();

            builder.Property(sf => sf.FindGender)
                .HasConversion(
                    genders => string.Join(",", genders),
                    value => value.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList())
                .HasMaxLength(500);

            builder.Property(sf => sf.MinAge)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(sf => sf.MaxAge)
                .IsRequired()
                .HasDefaultValue(100);

            builder.Property(sf => sf.FindRegion)
                .HasConversion(
                    regions => string.Join(",", regions),
                    value => value.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList())
                .HasMaxLength(500);

            builder.Property(sf => sf.CreatedAt)
                .IsRequired();

            builder.Property(sf => sf.UpdatedAt);

            builder.HasIndex(sf => sf.MinAge);
            builder.HasIndex(sf => sf.MaxAge);
            builder.HasIndex(sf => sf.CreatedAt);
        }
    }
}
