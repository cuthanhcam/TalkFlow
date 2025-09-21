using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.Room
{
    public record RoomCapacity
    {
        public int Value { get; }

        public RoomCapacity() : this(0) { }

        public RoomCapacity(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Room capacity cannot be negative", nameof(value));
            }

            if (value > 100)
            {
                throw new ArgumentException("Room capacity cannot exceed 100", nameof(value));
            }

            Value = value;
        }

        public static implicit operator int(RoomCapacity capacity) => capacity.Value;
        public static implicit operator RoomCapacity(int value) => new(value);

        public override string ToString() => Value.ToString();
    }
}
