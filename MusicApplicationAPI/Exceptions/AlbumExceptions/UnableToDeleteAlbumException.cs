namespace MusicApplicationAPI.Exceptions.AlbumExceptions
{
    [Serializable]
    public class UnableToDeleteAlbumException : Exception
    {
        private string msg;
        public UnableToDeleteAlbumException()
        {
            msg = "Something went wrong while deleting the album.";
        }

        public UnableToDeleteAlbumException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
