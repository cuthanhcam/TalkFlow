using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.ValueObjects.Room
{
    public record RoomName
    {
        public string Value { get; }

        public RoomName() : this("Default Room") { }

        public RoomName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Room name cannot be null or empty", nameof(value));
            }

            if (value.Length < 3)
            {
                throw new ArgumentException("Room name must be at least 3 characters", nameof(value));
            }

            if (value.Length > 100)
            {
                throw new ArgumentException("Room name cannot exceed 100 characters", nameof(value));
            }

            Value = value.Trim();
        }

        public static implicit operator string(RoomName roomName) => roomName.Value;
        public static implicit operator RoomName(string value) => new(value);

        public override string ToString() => Value;
    }
}
