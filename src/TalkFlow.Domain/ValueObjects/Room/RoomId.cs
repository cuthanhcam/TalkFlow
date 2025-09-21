using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.Common;

namespace TalkFlow.Domain.ValueObjects.Room
{
    public record RoomId : EntityId<RoomId>
    {
        public RoomId() : base(Guid.NewGuid()) { }

        public RoomId(Guid value) : base(value) { }

        public static RoomId New() => new(Guid.NewGuid());
    }
}
