using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Events;
using TalkFlow.Domain.ValueObjects.Room;

namespace TalkFlow.Domain.Aggregates.Room.Events
{
    public record MessageSentEvent(RoomId RoomId, Message Message) : DomainEventBase;
}
