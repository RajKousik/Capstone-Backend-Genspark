using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApplicationAPI.Models.DbModels
{
    public class PremiumUser
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Money { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
