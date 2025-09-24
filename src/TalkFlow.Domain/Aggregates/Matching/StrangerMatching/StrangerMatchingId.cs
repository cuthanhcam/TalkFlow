using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.Aggregates.Matching.StrangerMatching
{
    public record StrangerMatchingId : ValueObjects.Common.EntityId<StrangerMatchingId> // value object
    {
        public StrangerMatchingId() : base(Guid.NewGuid()) { }
        public StrangerMatchingId(Guid value) : base(value) { }
        public static StrangerMatchingId New() => new(Guid.NewGuid());
    }
}
