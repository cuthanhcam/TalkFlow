using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.Aggregates.Matching.Match
{
    public record MatchId : ValueObjects.Common.EntityId<MatchId> // value object
    {
        public MatchId() : base(Guid.NewGuid()) { }
        public MatchId(Guid value) : base(value) { }
        public static MatchId New() => new(Guid.NewGuid());
    }
}
