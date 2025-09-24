using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.Specifications.Room
{
    public class RoomByCapacitySpecification : BaseSpecification<TalkFlow.Domain.Aggregates.Room.Room>
    {
        public RoomByCapacitySpecification(int minCapacity, int maxCapacity)
            : base(r => r.Capacity.Value >= minCapacity && r.Capacity.Value <= maxCapacity)
        {
        }
    }
}
