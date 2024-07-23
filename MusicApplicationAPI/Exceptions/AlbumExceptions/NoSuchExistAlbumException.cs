namespace MusicApplicationAPI.Exceptions.AlbumExceptions
{
    [Serializable]
    public class NoSuchAlbumExistException : Exception
    {
        private string msg;
        public NoSuchAlbumExistException()
        {
            msg = "Album does not exist.";
        }

        public NoSuchAlbumExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
