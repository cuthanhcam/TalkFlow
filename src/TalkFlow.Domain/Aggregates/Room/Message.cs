using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Aggregates.Room
{
    public class Message // entity
    {
        public Guid Id { get; private set; }
        public Guid RoomId { get; private set; }
        public UserId SenderId { get; private set; }
        public string SenderDisplayName { get; private set; }
        public string Content { get; private set; }
        public DateTime SentAt { get; private set; }
        public bool IsDeleted { get; private set; }

        protected Message()
        {
            RoomId = Guid.NewGuid();
            SenderId = UserId.New();
            SenderDisplayName = string.Empty;
            Content = string.Empty;
        }

        public Message(Guid roomId, UserId senderId, string senderDisplayName, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Message content cannot be null or empty", nameof(content));

            if (content.Length > 1000)
                throw new ArgumentException("Message content cannot exceed 1000 characters", nameof(content));

            Id = Guid.NewGuid();
            RoomId = roomId;
            SenderId = senderId;
            SenderDisplayName = senderDisplayName ?? throw new ArgumentNullException(nameof(senderDisplayName));
            Content = content.Trim();
            SentAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
