namespace MusicApplicationAPI.Exceptions.FavoriteExceptions
{
    [Serializable]
    public class NoSuchFavoriteExistException : Exception
    {
        private string msg;
        public NoSuchFavoriteExistException()
        {
            msg = "Favorite does not exist.";
        }

        public NoSuchFavoriteExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
