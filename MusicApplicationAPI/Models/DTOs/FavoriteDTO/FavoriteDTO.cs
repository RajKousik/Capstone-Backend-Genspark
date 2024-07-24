namespace MusicApplicationAPI.Models.DTOs.FavoriteDTO
{
    public class FavoriteDTO
    {
        public int UserId { get; set; }

        public int? SongId { get; set; }

        public int? PlaylistId { get; set; }
    }
}
