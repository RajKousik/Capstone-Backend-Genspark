namespace MusicApplicationAPI.Exceptions.PlaylistSongExceptions
{
    [Serializable]
    public class NoSuchPlaylistSongException : Exception
    {
        private string msg;
        public NoSuchPlaylistSongException()
        {
            msg = "Playlist-Song relationship does not exist.";
        }

        public NoSuchPlaylistSongException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
