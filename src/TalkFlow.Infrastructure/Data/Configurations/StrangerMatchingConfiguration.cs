using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Matching.StrangerMatching;

namespace TalkFlow.Infrastructure.Data.Configurations
{
    public class StrangerMatchingConfiguration : IEntityTypeConfiguration<StrangerMatching>
    {
        public void Configure(EntityTypeBuilder<StrangerMatching> builder)
        {
            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.Id)
                .HasConversion(id => id.Value, value => new StrangerMatchingId(value));

            builder.Property(sm => sm.UserId)
                .HasConversion(id => id.Value, value => new Domain.ValueObjects.User.UserId(value));

            builder.Property(sm => sm.Status)
                .HasConversion(status => status.ToString(),
                              value => Enum.Parse<StrangerMatchingStatus>(value));

            builder.Property(sm => sm.StartedAt)
                .IsRequired();

            builder.Property(sm => sm.MatchingCriteria)
                .HasMaxLength(500);

            builder.Property(sm => sm.Attempts)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasIndex(sm => sm.UserId);
            builder.HasIndex(sm => sm.Status);
            builder.HasIndex(sm => sm.StartedAt);
        }
    }
}
