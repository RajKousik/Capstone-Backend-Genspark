namespace MusicApplicationAPI.Models.DTOs.FavoriteDTO
{
    public class FavoriteReturnDTO
    {
        public int FavoriteId { get; set; }
        public int UserId { get; set; }
        public int? SongId { get; set; }
        public int? PlaylistId { get; set; }
    }
}
