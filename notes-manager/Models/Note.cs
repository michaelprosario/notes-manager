using System;
using System.ComponentModel.DataAnnotations;

namespace NotesManager.Models
{
    public class Note
    {
        [Key]
        public string NoteId { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Note name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Note name must be between 1 and 200 characters")]
        [RegularExpression(@"^[^<>:""/\\|?*]+$", ErrorMessage = "Note name contains invalid characters")]
        public string NoteName { get; set; } = string.Empty;

        [MaxLength(1000000, ErrorMessage = "Note content is too large")]
        public string? Content { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }
}
