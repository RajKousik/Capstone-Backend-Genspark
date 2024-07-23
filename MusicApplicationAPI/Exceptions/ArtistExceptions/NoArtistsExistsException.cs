namespace MusicApplicationAPI.Exceptions.ArtistExceptions
{
    [Serializable]
    public class NoArtistsExistsException : Exception
    {
        private string msg;
        public NoArtistsExistsException()
        {
            msg = "No artists exist in the database.";
        }

        public NoArtistsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
