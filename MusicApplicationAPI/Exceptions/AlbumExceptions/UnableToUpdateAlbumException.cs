namespace MusicApplicationAPI.Exceptions.AlbumExceptions
{
    [Serializable]
    public class UnableToUpdateAlbumException : Exception
    {
        private string msg;
        public UnableToUpdateAlbumException()
        {
            msg = "Something went wrong while updating the album.";
        }

        public UnableToUpdateAlbumException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
