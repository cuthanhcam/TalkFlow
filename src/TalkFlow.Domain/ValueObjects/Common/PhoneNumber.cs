using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.Common
{
    public record PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone number cannot be empty.", nameof(value));

            // Basic validation for phone number format (E.164 format) - can be adjusted as needed
            var cleaned = value.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            if (!System.Text.RegularExpressions.Regex.IsMatch(cleaned, @"^\+?[1-9]\d{1,14}$"))
                throw new ArgumentException("Invalid phone number format", nameof(value));

            Value = cleaned;
        }

        public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
        public static implicit operator PhoneNumber(string value) => new(value);

        public override string ToString() => Value;
    }
}
