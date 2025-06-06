using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for notification table
    /// </summary>
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Type { get; set; }

        public bool? IsRead { get; set; } = false;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User User { get; set; }
    }
}
