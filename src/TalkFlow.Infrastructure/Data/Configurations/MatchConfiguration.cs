using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Matching.Match;

namespace TalkFlow.Infrastructure.Data.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(id => id.Value, value => new MatchId(value));

            builder.Property(m => m.User1Id)
                .HasConversion(id => id.Value, value => new Domain.ValueObjects.User.UserId(value));

            builder.Property(m => m.User2Id)
                .HasConversion(id => id.Value, value => new Domain.ValueObjects.User.UserId(value));

            builder.Property(m => m.RoomId)
                .HasConversion(id => id != null ? id.Value : (Guid?)null,
                              value => value.HasValue ? new Domain.ValueObjects.Room.RoomId(value.Value) : null);

            builder.Property(m => m.Status)
                .HasConversion(status => status.ToString(),
                              value => Enum.Parse<MatchStatus>(value));

            builder.Property(m => m.MatchedAt)
                .IsRequired();

            builder.Property(m => m.MatchingCriteria)
                .HasMaxLength(500);

            builder.HasIndex(m => m.User1Id);
            builder.HasIndex(m => m.User2Id);
            builder.HasIndex(m => m.Status);
            builder.HasIndex(m => m.MatchedAt);
        }
    }
}
