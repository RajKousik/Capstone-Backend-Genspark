namespace MusicApplicationAPI.Exceptions.PlaylistSongExceptions
{
    [Serializable]
    public class NoPlaylistSongsExistsException : Exception
    {
        private string msg;
        public NoPlaylistSongsExistsException()
        {
            msg = "No Playlist songs exist in the database.";
        }

        public NoPlaylistSongsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
