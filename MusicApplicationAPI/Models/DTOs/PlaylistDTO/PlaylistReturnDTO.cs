namespace MusicApplicationAPI.Models.DTOs.PlaylistDTO
{
    public class PlaylistReturnDTO
    {
        public int PlaylistId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}
