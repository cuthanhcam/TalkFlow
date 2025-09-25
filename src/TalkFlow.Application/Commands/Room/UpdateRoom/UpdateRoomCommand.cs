using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Common.Interfaces;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Room;
using TalkFlow.Domain.ValueObjects.Room;

namespace TalkFlow.Application.Commands.Room.UpdateRoom
{
    public record UpdateRoomCommand(RoomId RoomId, UpdateRoomDto RoomData) : ICommand<Result<RoomDto>>;
}
