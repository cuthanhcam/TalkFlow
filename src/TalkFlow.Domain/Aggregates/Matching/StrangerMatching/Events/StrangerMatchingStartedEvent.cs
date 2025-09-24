using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Events;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Aggregates.Matching.StrangerMatching.Events
{
    public record StrangerMatchingStartedEvent(StrangerMatchingId StrangerMatchingId, UserId UserId) : DomainEventBase;
}
