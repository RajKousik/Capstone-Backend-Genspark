namespace MusicApplicationAPI.Exceptions.PlaylistExceptions
{
    [Serializable]
    public class UnableToAddPlaylistException : Exception
    {
        private string msg;
        public UnableToAddPlaylistException()
        {
            msg = "Something went wrong while adding a playlist record";
        }

        public UnableToAddPlaylistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
