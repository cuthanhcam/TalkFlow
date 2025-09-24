using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Aggregates.Room
{
    public class Connection
    {
        public string ConnectionId { get; private set; }
        public UserId UserId { get; private set; }
        public Guid RoomId { get; private set; }
        public DateTime ConnectedAt { get; private set; }

        protected Connection()
        {
            ConnectionId = string.Empty;
            UserId = UserId.New();
            RoomId = Guid.NewGuid();
        }

        public Connection(string connectionId, UserId userId, Guid roomId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new ArgumentException("Connection ID cannot be null or empty", nameof(connectionId));

            ConnectionId = connectionId;
            UserId = userId;
            RoomId = roomId;
            ConnectedAt = DateTime.UtcNow;
        }
    }
}
