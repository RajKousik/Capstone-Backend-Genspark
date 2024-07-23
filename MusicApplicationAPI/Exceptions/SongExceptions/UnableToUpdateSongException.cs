namespace MusicApplicationAPI.Exceptions.SongExceptions
{
    [Serializable]
    public class UnableToUpdateSongException : Exception
    {
        private string msg;
        public UnableToUpdateSongException()
        {
            msg = "Something went wrong while updating a song record";
        }

        public UnableToUpdateSongException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
