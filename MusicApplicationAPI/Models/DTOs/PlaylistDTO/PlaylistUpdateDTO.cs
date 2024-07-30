using System.ComponentModel.DataAnnotations;

namespace MusicApplicationAPI.Models.DTOs.PlaylistDTO
{
    public class PlaylistUpdateDTO
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }

        public string ImageUrl { get; set; }
    }
}
