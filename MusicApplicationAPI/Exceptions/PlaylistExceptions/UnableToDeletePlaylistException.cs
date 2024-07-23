namespace MusicApplicationAPI.Exceptions.PlaylistExceptions
{
    [Serializable]
    public class UnableToDeletePlaylistException : Exception
    {
        private string msg;
        public UnableToDeletePlaylistException()
        {
            msg = "Something went wrong while deleting a playlist record";
        }

        public UnableToDeletePlaylistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
