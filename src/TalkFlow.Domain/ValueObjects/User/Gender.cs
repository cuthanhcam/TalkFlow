using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.User
{
    public record Gender
    {
        public static readonly string[] ValidGenders = { "Male", "Female", "Other", "PreferNotToSay" };

        public string Value { get; }

        public Gender() : this("PreferNotToSay") { } // Default

        public Gender(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Gender cannot be null or empty", nameof(value));
            }

            if (!ValidGenders.Contains(value, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Invalid gender. Must be one of: {string.Join(", ", ValidGenders)}", nameof(value));
            }

            Value = value;
        }

        public static implicit operator string(Gender gender) => gender.Value;
        public static implicit operator Gender(string value) => new(value);

        public override string ToString() => Value;
    }
}
