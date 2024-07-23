namespace MusicApplicationAPI.Exceptions.ArtistExceptions
{
    [Serializable]
    public class NoSuchArtistExistException : Exception
    {
        private string msg;
        public NoSuchArtistExistException()
        {
            msg = "Artist does not exist.";
        }

        public NoSuchArtistExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
