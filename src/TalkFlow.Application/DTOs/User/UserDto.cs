using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Application.DTOs.User
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public DateTime LastActive { get; set; }
        public bool IsLocked { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? Nationality { get; set; }
        public StrangerFilterDto? StrangerFilter { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
