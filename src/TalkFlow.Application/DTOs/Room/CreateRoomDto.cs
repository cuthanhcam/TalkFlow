using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Application.DTOs.Room
{
    public class CreateRoomDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string RoomName { get; set; } = string.Empty;

        [StringLength(20, MinimumLength = 4)]
        public string? SecurityCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string DisplayName { get; set; } = string.Empty;
    }
}
