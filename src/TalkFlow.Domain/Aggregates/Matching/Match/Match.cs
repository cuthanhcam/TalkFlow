using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Matching.Match.Events;
using TalkFlow.Domain.Events;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Aggregates.Matching.Match
{
    public class Match // aggregate root
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public MatchId Id { get; private set; }
        public UserId User1Id { get; private set; }
        public UserId User2Id { get; private set; }
        public RoomId? RoomId { get; private set; }
        public DateTime MatchedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public MatchStatus Status { get; private set; }
        public string? MatchingCriteria { get; private set; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected Match()
        {
            Id = MatchId.New();
            User1Id = UserId.New();
            User2Id = UserId.New();
        }

        public Match(UserId user1Id, UserId user2Id, string? matchingCriteria = null)
        {
            Id = MatchId.New();
            User1Id = user1Id;
            User2Id = user2Id;
            MatchedAt = DateTime.UtcNow;
            Status = MatchStatus.Pending;
            MatchingCriteria = matchingCriteria;

            AddDomainEvent(new MatchCreatedEvent(Id, User1Id, User2Id));
        }

        public static Match Create(UserId user1Id, UserId user2Id, string? matchingCriteria = null)
        {
            return new Match(user1Id, user2Id, matchingCriteria);
        }

        public void AssignRoom(RoomId roomId)
        {
            if (Status != MatchStatus.Pending)
                throw new InvalidOperationException("Cannot assign room to non-pending match");

            RoomId = roomId;
        }

        public void Complete()
        {
            if (Status != MatchStatus.Pending)
                throw new InvalidOperationException("Cannot complete non-pending match");

            Status = MatchStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            AddDomainEvent(new MatchCompletedEvent(Id, User1Id, User2Id, RoomId));
        }

        public void Cancel()
        {
            if (Status == MatchStatus.Completed)
                throw new InvalidOperationException("Cannot cancel completed match");

            Status = MatchStatus.Cancelled;
        }

        public bool IsActive => Status == MatchStatus.Pending;
        public bool IsCompleted => Status == MatchStatus.Completed;
        public bool IsCancelled => Status == MatchStatus.Cancelled;

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }

    public enum MatchStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}
