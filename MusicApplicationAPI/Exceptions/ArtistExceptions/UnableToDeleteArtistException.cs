namespace MusicApplicationAPI.Exceptions.ArtistExceptions
{
    [Serializable]
    public class UnableToDeleteArtistException : Exception
    {
        private string msg;
        public UnableToDeleteArtistException()
        {
            msg = "Something went wrong while deleting the artist.";
        }

        public UnableToDeleteArtistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
