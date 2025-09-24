using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.Room;

namespace TalkFlow.Domain.Specifications.Room
{
    public class RoomBySecurityCodeSpecification : BaseSpecification<TalkFlow.Domain.Aggregates.Room.Room>
    {
        public RoomBySecurityCodeSpecification(SecurityCode securityCode)
            : base(r => r.SecurityCode.Value == securityCode.Value)
        {
        }
    }

}
