namespace MusicApplicationAPI.Exceptions.AlbumExceptions
{
    [Serializable]
    public class UnableToAddAlbumException : Exception
    {
        private string msg;
        public UnableToAddAlbumException()
        {
            msg = "Something went wrong while adding the album.";
        }

        public UnableToAddAlbumException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
