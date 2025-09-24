using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TalkFlow.Domain.Events;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Aggregates.Matching.Match.Events
{
    public record MatchCompletedEvent(MatchId MatchId, UserId User1Id, UserId User2Id, RoomId? RoomId) : DomainEventBase;
}
