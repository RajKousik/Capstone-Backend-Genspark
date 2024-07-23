namespace MusicApplicationAPI.Exceptions.PlaylistSongExceptions
{
    [Serializable]
    public class UnableToAddPlaylistSongException : Exception
    {
        private string msg;
        public UnableToAddPlaylistSongException()
        {
            msg = "Something went wrong while adding the playlist-song relationship.";
        }

        public UnableToAddPlaylistSongException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
