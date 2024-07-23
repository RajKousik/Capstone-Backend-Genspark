namespace MusicApplicationAPI.Exceptions.AlbumExceptions
{
    [Serializable]
    public class NoAlbumsExistsException : Exception
    {
        private string msg;
        public NoAlbumsExistsException()
        {
            msg = "No albums exist in the database.";
        }

        public NoAlbumsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
