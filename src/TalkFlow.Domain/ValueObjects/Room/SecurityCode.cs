using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.Room
{
    public record SecurityCode
    {
        public string Value { get; }
        public static SecurityCode Empty => new(string.Empty);

        public SecurityCode() : this(string.Empty) { }

        public SecurityCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Value = string.Empty;
                return;
            }

            if (value.Length < 4)
            {
                throw new ArgumentException("Security code must be at least 4 characters", nameof(value));
            }

            if (value.Length > 20)
            {
                throw new ArgumentException("Security code cannot exceed 20 characters", nameof(value));
            }

            Value = value.Trim();
        }

        public static implicit operator string(SecurityCode securityCode) => securityCode.Value;
        public static implicit operator SecurityCode(string value) => new(value);

        public override string ToString() => Value;

        public bool IsEmpty => string.IsNullOrEmpty(Value);
    }
}
