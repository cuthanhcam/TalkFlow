using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.Common.Interfaces;
using TalkFlow.Application.DTOs.Common;
using TalkFlow.Application.DTOs.Room;
using TalkFlow.Application.DTOs.User;

namespace TalkFlow.Application.Commands.Room.CreateRoom
{
    public record CreateRoomCommand(CreateRoomDto RoomData) : ICommand<Result<CreateRoomResponseDto>>;

    public record CreateRoomResponseDto
    {
        public UserDto User { get; set; } = new();
        public RoomDto Room { get; set; } = new();
    }
}
