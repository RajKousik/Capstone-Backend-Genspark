namespace MusicApplicationAPI.Exceptions.SongExceptions
{
    [Serializable]
    public class UnableToDeleteSongException : Exception
    {
        private string msg;
        public UnableToDeleteSongException()
        {
            msg = "Something went wrong while deleting a song record";
        }

        public UnableToDeleteSongException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
