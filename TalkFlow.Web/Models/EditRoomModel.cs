using System.ComponentModel.DataAnnotations;
namespace TalkFlow.Models
{
    public class EditRoomModel
    {
        [Required]
        public Guid RoomId { get; set; }

        [Required]
        public string RoomName { get; set; } = null!;

        public string? SecurityCode { get; set; }
    }
}
