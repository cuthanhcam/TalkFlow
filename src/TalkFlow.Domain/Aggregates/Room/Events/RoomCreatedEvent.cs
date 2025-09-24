using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.Events;
using TalkFlow.Domain.ValueObjects.Room;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Aggregates.Room.Events
{
    public record RoomCreatedEvent(RoomId RoomId, RoomName RoomName, UserId HostId) : DomainEventBase;
}
