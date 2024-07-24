using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.FavoriteExceptions
{
    [Serializable]
    public class UnableToRetrieveFavoritesException : Exception
    {
        private string msg;
        public UnableToRetrieveFavoritesException()
        {
            msg = "Unable to retrieve favorite songs.";
        }

        public UnableToRetrieveFavoritesException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}