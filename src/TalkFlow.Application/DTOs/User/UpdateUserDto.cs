using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Application.DTOs.User
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string DisplayName { get; set; } = string.Empty;

        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? Nationality { get; set; }
        public string? PhotoUrl { get; set; }
        public StrangerFilterDto? StrangerFilter { get; set; }
    }
}
