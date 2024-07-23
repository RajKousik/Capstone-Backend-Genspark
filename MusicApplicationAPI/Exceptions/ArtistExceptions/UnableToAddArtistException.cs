namespace MusicApplicationAPI.Exceptions.ArtistExceptions
{
    [Serializable]
    public class UnableToAddArtistException : Exception
    {
        private string msg;
        public UnableToAddArtistException()
        {
            msg = "Something went wrong while adding the artist.";
        }

        public UnableToAddArtistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
