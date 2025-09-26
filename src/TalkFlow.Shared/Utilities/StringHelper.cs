using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TalkFlow.Shared.Utilities
{
    public static class StringHelper
    {
        public static bool IsNullOrEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
                return value;

            return char.ToLowerInvariant(value[0]) + value[1..];
        }

        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || char.IsUpper(value[0]))
                return value;

            return char.ToUpperInvariant(value[0]) + value[1..];
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;

            return value[..maxLength];
        }

        public static string RemoveSpecialCharacters(this string value)
        {
            return Regex.Replace(value, @"[^a-zA-Z0-9\s]", "");
        }

        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string MaskEmail(this string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains('@'))
                return email;

            var parts = email.Split('@');
            var username = parts[0];
            var domain = parts[1];

            if (username.Length <= 2)
                return $"{username[0]}*@{domain}";

            return $"{username[0]}{new string('*', username.Length - 2)}{username[^1]}@{domain}";
        }
    }
}
