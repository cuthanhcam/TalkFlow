using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Application.DTOs.User;

namespace TalkFlow.Application.DTOs.Room
{
    public class JoinRoomResponseDto
    {
        public UserDto User { get; set; } = new();
        public RoomDto Room { get; set; } = new();
    }
}
