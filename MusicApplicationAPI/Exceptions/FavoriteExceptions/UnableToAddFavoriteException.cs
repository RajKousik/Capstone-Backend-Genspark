namespace MusicApplicationAPI.Exceptions.FavoriteExceptions
{
    [Serializable]
    public class UnableToAddFavoriteException : Exception
    {
        private string msg;
        public UnableToAddFavoriteException()
        {
            msg = "Something went wrong while adding the favorite.";
        }

        public UnableToAddFavoriteException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
