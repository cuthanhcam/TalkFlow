using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Application.DTOs.Room
{
    public class RoomDto
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public Guid HostId { get; set; }
        public string HostDisplayName { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsChatBlocked { get; set; }
        public bool IsActive { get; set; }
        public string? SecurityCode { get; set; }
    }
}
