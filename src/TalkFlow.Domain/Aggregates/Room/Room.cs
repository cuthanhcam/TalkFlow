using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Room.Events;
using TalkFlow.Domain.Events;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Aggregates.Room
{
    public class Room // aggregate root
    {
        private readonly List<Connection> _connections = new();
        private readonly List<Message> _messages = new();
        private readonly List<IDomainEvent> _domainEvents = new();

        public Guid Id { get; private set; }
        public RoomName Name { get; private set; }
        public SecurityCode SecurityCode { get; private set; }
        public RoomCapacity Capacity { get; private set; }
        public UserId HostId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsChatBlocked { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyCollection<Connection> Connections => _connections.AsReadOnly();
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected Room()
        {
            Id = Guid.NewGuid();
            Name = new RoomName("Default");
            SecurityCode = SecurityCode.Empty;
            Capacity = new RoomCapacity(0);
            HostId = UserId.New();
        }

        public Room(RoomName name, UserId hostId, SecurityCode? securityCode = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            HostId = hostId;
            SecurityCode = securityCode ?? SecurityCode.Empty;
            Capacity = new RoomCapacity(0);
            CreatedAt = DateTime.UtcNow;
            IsChatBlocked = false;
            IsActive = true;

            AddDomainEvent(new RoomCreatedEvent(new RoomId(Id), Name, HostId));
        }

        public static Room Create(RoomName name, UserId hostId, SecurityCode? securityCode = null)
        {
            return new Room(name, hostId, securityCode);
        }

        public void AddConnection(Connection connection)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot add connection to inactive room");

            if (_connections.Any(c => c.ConnectionId == connection.ConnectionId))
                return;

            _connections.Add(connection);
            Capacity = new RoomCapacity(_connections.Count);
            AddDomainEvent(new UserJoinedRoomEvent(new RoomId(Id), connection.UserId));
        }

        public void RemoveConnection(string connectionId)
        {
            var connection = _connections.FirstOrDefault(c => c.ConnectionId == connectionId);
            if (connection == null) return;

            _connections.Remove(connection);
            Capacity = new RoomCapacity(_connections.Count);
            AddDomainEvent(new UserLeftRoomEvent(new RoomId(Id), connection.UserId));
        }

        public void AddMessage(Message message)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot add message to inactive room");

            if (IsChatBlocked)
                throw new InvalidOperationException("Cannot add message to blocked chat room");

            _messages.Add(message);
            AddDomainEvent(new MessageSentEvent(new RoomId(Id), message));
        }

        public void BlockChat()
        {
            if (IsChatBlocked) return;

            IsChatBlocked = true;
            AddDomainEvent(new ChatBlockedEvent(new RoomId(Id)));
        }

        public void UnblockChat()
        {
            if (!IsChatBlocked) return;

            IsChatBlocked = false;
            AddDomainEvent(new ChatUnblockedEvent(new RoomId(Id)));
        }

        public void Deactivate()
        {
            if (!IsActive) return;

            IsActive = false;
            AddDomainEvent(new RoomDeactivatedEvent(new RoomId(Id)));
        }

        public void UpdateName(RoomName name)
        {
            Name = name;
        }

        public void UpdateSecurityCode(SecurityCode securityCode)
        {
            SecurityCode = securityCode;
        }

        public bool CanUserJoin(UserId userId)
        {
            return IsActive && !_connections.Any(c => c.UserId == userId);
        }

        public bool IsHost(UserId userId)
        {
            return HostId == userId;
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
