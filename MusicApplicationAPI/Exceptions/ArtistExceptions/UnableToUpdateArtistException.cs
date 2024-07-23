namespace MusicApplicationAPI.Exceptions.ArtistExceptions
{
    [Serializable]
    public class UnableToUpdateArtistException : Exception
    {
        private string msg;
        public UnableToUpdateArtistException()
        {
            msg = "Something went wrong while updating the artist.";
        }

        public UnableToUpdateArtistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
