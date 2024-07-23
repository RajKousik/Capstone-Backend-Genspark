namespace MusicApplicationAPI.Exceptions.SongExceptions
{
    [Serializable]
    public class NoSongsExistsException : Exception
    {
        private string msg;
        public NoSongsExistsException()
        {
            msg = "No songs Found in the database";
        }

        public NoSongsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
