using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Aggregates.Matching.StrangerMatching.Events;
using TalkFlow.Domain.Events;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Aggregates.Matching.StrangerMatching
{
    public class StrangerMatching
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public StrangerMatchingId Id { get; private set; }
        public UserId UserId { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? StoppedAt { get; private set; }
        public StrangerMatchingStatus Status { get; private set; }
        public string? MatchingCriteria { get; private set; }
        public int Attempts { get; private set; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected StrangerMatching()
        {
            Id = StrangerMatchingId.New();
            UserId = UserId.New();
        }

        public StrangerMatching(UserId userId, string? matchingCriteria = null)
        {
            Id = StrangerMatchingId.New();
            UserId = userId;
            StartedAt = DateTime.UtcNow;
            Status = StrangerMatchingStatus.Active;
            MatchingCriteria = matchingCriteria;
            Attempts = 0;

            AddDomainEvent(new StrangerMatchingStartedEvent(Id, UserId));
        }

        public static StrangerMatching Start(UserId userId, string? matchingCriteria = null)
        {
            return new StrangerMatching(userId, matchingCriteria);
        }

        public void Stop()
        {
            if (Status != StrangerMatchingStatus.Active)
                throw new InvalidOperationException("Cannot stop non-active matching");

            Status = StrangerMatchingStatus.Stopped;
            StoppedAt = DateTime.UtcNow;
            AddDomainEvent(new StrangerMatchingStoppedEvent(Id, UserId));
        }

        public void IncrementAttempts()
        {
            Attempts++;
        }

        public void Complete()
        {
            if (Status != StrangerMatchingStatus.Active)
                throw new InvalidOperationException("Cannot complete non-active matching");

            Status = StrangerMatchingStatus.Completed;
            StoppedAt = DateTime.UtcNow;
            AddDomainEvent(new StrangerMatchedEvent(Id, UserId));
        }

        public bool IsActive => Status == StrangerMatchingStatus.Active;
        public bool IsStopped => Status == StrangerMatchingStatus.Stopped;
        public bool IsCompleted => Status == StrangerMatchingStatus.Completed;

        public TimeSpan Duration => (StoppedAt ?? DateTime.UtcNow) - StartedAt;

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }

    public enum StrangerMatchingStatus
    {
        Active,
        Stopped,
        Completed
    }
}
