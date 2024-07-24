using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.PlaylistDTO
{
    public class PlaylistAddDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}
