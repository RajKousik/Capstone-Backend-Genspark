namespace MusicApplicationAPI.Exceptions.PlaylistExceptions
{
    [Serializable]
    public class NoSuchPlaylistExistException : Exception
    {
        private string msg;
        public NoSuchPlaylistExistException()
        {
            msg = "The specified playlist does not exist";
        }

        public NoSuchPlaylistExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
