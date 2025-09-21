using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.User
{
    public record PhotoUrl
    {
        private static readonly Regex UrlRegex = new(
            @"^https?://[^\s/$.?#].[^\s]*$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }
        public static PhotoUrl Empty => new(string.Empty);

        public PhotoUrl() : this(string.Empty) { }

        public PhotoUrl(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Value = string.Empty; return;
            }

            if (!UrlRegex.IsMatch(value))
            {
                throw new ArgumentException("Invalid photo URL format", nameof(value));
            }

            Value = value;
        }

        public static implicit operator string(PhotoUrl photoUrl) => photoUrl.Value;
        public static implicit operator PhotoUrl(string value) => new(value);

        public override string ToString() => Value;

        public bool IsEmpty => string.IsNullOrEmpty(Value);
    }
}
