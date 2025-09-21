using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.User
{
    public record Age
    {
        public int Value { get; }
        
        public Age() : this(18) { } // Default age is set to 18 - can be adjusted as needed

        public Age(int value)
        {
            if (value < 13)
            {
                throw new ArgumentOutOfRangeException("Age must be at least 13", nameof(value));
            }

            if (value > 120)
            {
                throw new ArgumentOutOfRangeException("Age cannot exceed 120", nameof(value));
            }

            Value = value;
        }

        public static implicit operator int(Age age) => age.Value;
        public static implicit operator Age(int value) => new(value);

        public override string ToString() => Value.ToString();
    }
}
