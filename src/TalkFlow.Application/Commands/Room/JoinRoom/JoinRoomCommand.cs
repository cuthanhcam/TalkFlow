using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Common.Interfaces;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Room;

namespace TalkFlow.Application.Commands.Room.JoinRoom
{
    public record JoinRoomCommand(JoinRoomDto RoomData) : ICommand<Result<RoomDto>>;
}
