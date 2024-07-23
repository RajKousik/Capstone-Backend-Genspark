namespace MusicApplicationAPI.Exceptions.PlaylistSongExceptions
{
    [Serializable]
    public class UnableToUpdatePlaylistSongException : Exception
    {
        private string msg;
        public UnableToUpdatePlaylistSongException()
        {
            msg = "Something went wrong while updating the playlist-song relationship.";
        }

        public UnableToUpdatePlaylistSongException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
