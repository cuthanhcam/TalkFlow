using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Application.DTOs.Room
{
    public class UpdateRoomDto
    {
        public string RoomName { get; set; } = string.Empty;
        public string? SecurityCode { get; set; }
    }
}
