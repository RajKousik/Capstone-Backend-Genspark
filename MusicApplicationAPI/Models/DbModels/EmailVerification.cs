using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DbModels
{
    public class EmailVerification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string? VerificationCode { get; set; }

        public DateTime ExpiryDate { get; set; }

        public User? User { get; set; }
    }
}
