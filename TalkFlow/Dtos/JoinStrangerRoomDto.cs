using System.ComponentModel.DataAnnotations;

namespace TalkFlow.Dtos
{
    public class JoinStrangerRoomDto
    {
        [Required]
        public Guid RoomId { get; set; }

        public string? SecurityCode { get; set; } = null!;
    }
}


