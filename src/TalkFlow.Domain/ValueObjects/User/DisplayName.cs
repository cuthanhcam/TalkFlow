using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.User
{
    public record DisplayName
    {
        public string Value { get; }

        public DisplayName() : this("Default") { } // Default display name is set to "Default" - can be adjusted as needed

        public DisplayName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Display name cannot be null or empty", nameof(value));
            }

            if (value.Length < 3)
            {
                throw new ArgumentException("Display name must be at least 3 characters", nameof(value));
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("Display name cannot exceed 50 characters", nameof(value));
            }

            Value = value.Trim(); // Trim whitespace
        }

        public static implicit operator string(DisplayName displayName) => displayName.Value;
        public static implicit operator DisplayName(string value) => new(value);

        public override string ToString() => Value;
    }
}
