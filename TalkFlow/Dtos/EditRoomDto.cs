using System.ComponentModel.DataAnnotations;

namespace TalkFlow.Dtos
{
    public class EditRoomDto
    {
        [Required]
        public Guid RoomId { get; set; }

        [Required]
        public string RoomName { get; set; } = null!;

        public string? SecurityCode { get; set; }
    }
}


