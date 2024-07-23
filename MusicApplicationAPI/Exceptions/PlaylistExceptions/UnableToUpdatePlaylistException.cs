namespace MusicApplicationAPI.Exceptions.PlaylistExceptions
{
    [Serializable]
    public class UnableToUpdatePlaylistException : Exception
    {
        private string msg;
        public UnableToUpdatePlaylistException()
        {
            msg = "Something went wrong while updating a playlist record";
        }

        public UnableToUpdatePlaylistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
