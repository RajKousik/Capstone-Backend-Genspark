namespace MusicApplicationAPI.Exceptions.PlaylistSongExceptions
{
    [Serializable]
    public class NoSuchPlaylistSongExistException : Exception
    {
        private string msg;
        public NoSuchPlaylistSongExistException()
        {
            msg = "Playlist-Song relationship does not exist.";
        }

        public NoSuchPlaylistSongExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
