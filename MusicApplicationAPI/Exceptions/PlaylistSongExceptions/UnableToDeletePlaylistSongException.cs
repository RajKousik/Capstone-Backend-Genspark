namespace MusicApplicationAPI.Exceptions.PlaylistSongExceptions
{
    [Serializable]
    public class UnableToDeletePlaylistSongException : Exception
    {
        private string msg;
        public UnableToDeletePlaylistSongException()
        {
            msg = "Something went wrong while deleting the playlist-song relationship.";
        }

        public UnableToDeletePlaylistSongException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
