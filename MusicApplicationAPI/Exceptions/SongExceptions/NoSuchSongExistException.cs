namespace MusicApplicationAPI.Exceptions.SongExceptions
{
    [Serializable]
    public class NoSuchSongExistException : Exception
    {
        private string msg;
        public NoSuchSongExistException()
        {
            msg = "The specified song does not exist";
        }

        public NoSuchSongExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
