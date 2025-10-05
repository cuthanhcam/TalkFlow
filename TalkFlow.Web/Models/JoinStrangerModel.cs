
using System.ComponentModel.DataAnnotations;

namespace TalkFlow.Models
{
    public class JoinStrangerModel
    {
        [Required]
        public Guid RoomId { get; set; }

        public string? SecurityCode { get; set; } = null!;
    }
}
