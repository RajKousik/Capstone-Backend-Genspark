namespace MusicApplicationAPI.Exceptions.SongExceptions
{
    [Serializable]
    public class UnableToAddSongException : Exception
    {
        private string msg;
        public UnableToAddSongException()
        {
            msg = "Something went wrong while adding a song record";
        }

        public UnableToAddSongException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
