using System.ComponentModel.DataAnnotations;

namespace TalkFlow.Dtos
{
    /// <summary>
    /// Data transfer object for room creation
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Name of the room to create
        /// </summary>
        /// <example>My Video Room</example>
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string RoomName { get; set; }

        /// <summary>
        /// Display name of the room host
        /// </summary>
        /// <example>John Doe</example>
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Optional security code to protect the room
        /// </summary>
        /// <example>123456</example>
        public string? SecurityCode { get; set; }
    }

    /// <summary>
    /// Data transfer object for stranger session creation with filtering options
    /// </summary>
    public class RegisterStrangerDto : RegisterDto
    {
        /// <summary>
        /// Stranger matching filter criteria
        /// </summary>
        public StrangerFilterDto? StrangerFilter { get; set; } = null;
        
        /// <summary>
        /// Gender preference for matching
        /// </summary>
        /// <example>Male</example>
        public string? Gender { get; set; }
        
        /// <summary>
        /// Age for matching purposes
        /// </summary>
        /// <example>25</example>
        public int? Age { get; set; }
        
        /// <summary>
        /// Nationality for matching purposes
        /// </summary>
        /// <example>Vietnam</example>
        public string? Nationality { get; set; }
    }
}


