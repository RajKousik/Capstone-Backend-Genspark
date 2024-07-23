namespace MusicApplicationAPI.Exceptions.PlaylistExceptions
{
    [Serializable]
    public class NoPlaylistsExistsException : Exception
    {
        private string msg;
        public NoPlaylistsExistsException()
        {
            msg = "No Playlists Found in the database";
        }

        public NoPlaylistsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
