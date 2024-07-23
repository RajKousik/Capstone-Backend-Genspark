namespace MusicApplicationAPI.Exceptions.FavoriteExceptions
{
    [Serializable]
    public class UnableToDeleteFavoriteException : Exception
    {
        private string msg;
        public UnableToDeleteFavoriteException()
        {
            msg = "Something went wrong while deleting the favorite.";
        }

        public UnableToDeleteFavoriteException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
