using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.Common;

namespace TalkFlow.Domain.ValueObjects.User
{
    public record UserId : EntityId<UserId>
    {
        public UserId() : base(Guid.NewGuid()) { }

        public UserId(Guid value) : base(value) { }

        public static UserId New() => new(Guid.NewGuid());
    }
}
