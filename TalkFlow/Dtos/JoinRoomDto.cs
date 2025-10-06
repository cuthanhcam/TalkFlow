using System.ComponentModel.DataAnnotations;

namespace TalkFlow.Dtos
{
    public class JoinRoomDto
    {
        [Required]
        public Guid RoomId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string DisplayName { get; set; }

        public string? SecurityCode { get; set; } = null!;
    }
}


