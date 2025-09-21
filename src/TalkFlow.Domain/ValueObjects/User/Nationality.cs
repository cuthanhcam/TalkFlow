using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.User
{
    public record Nationality
    {
        public string Value { get; }

        public Nationality() : this("Unknown") { }

        public Nationality(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Nationality cannot be null or empty", nameof(value));
            }

            if (value.Length > 100)
            {
                throw new ArgumentException("Nationality cannot exceed 100 characters", nameof(value));
            }

            Value = value.Trim(); // Trim whitespace
        }

        public static implicit operator string(Nationality nationality) => nationality.Value;
        public static implicit operator Nationality(string value) => new(value);

        public override string ToString() => Value;
    }
}
