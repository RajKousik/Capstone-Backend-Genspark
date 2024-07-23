namespace MusicApplicationAPI.Exceptions.PlaylistExceptions
{
    [Serializable]
    public class NoSuchPlaylistException : Exception
    {
        private string msg;
        public NoSuchPlaylistException()
        {
            msg = "The specified playlist does not exist";
        }

        public NoSuchPlaylistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
