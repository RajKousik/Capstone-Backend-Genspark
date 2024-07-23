namespace MusicApplicationAPI.Exceptions.FavoriteExceptions
{
    [Serializable]
    public class UnableToUpdateFavoriteException : Exception
    {
        private string msg;
        public UnableToUpdateFavoriteException()
        {
            msg = "Something went wrong while updating the favorite.";
        }

        public UnableToUpdateFavoriteException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
