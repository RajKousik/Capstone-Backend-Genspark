using Microsoft.EntityFrameworkCore;
using MusicApplicationAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MusicApplicationAPI.Models.DbModels
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public RoleType Role { get; set; } // Admin, Normal User, Premium User

        [Required]
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }

        [Required]
        [JsonIgnore]
        public byte[] PasswordHashKey { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public string? Status { get; set; }
        public EmailVerification? EmailVerification { get; set; }

        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
    }
}
