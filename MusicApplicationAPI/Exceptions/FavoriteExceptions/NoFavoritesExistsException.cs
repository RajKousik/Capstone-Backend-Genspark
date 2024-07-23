namespace MusicApplicationAPI.Exceptions.FavoriteExceptions
{
    [Serializable]
    public class NoFavoritesExistsException : Exception
    {
        private string msg;
        public NoFavoritesExistsException()
        {
            msg = "No favorites exist in the database.";
        }

        public NoFavoritesExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
