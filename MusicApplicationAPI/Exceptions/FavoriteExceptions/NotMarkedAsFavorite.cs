using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.FavoriteExceptions
{
    [Serializable]
    public class NotMarkedAsFavorite : Exception
    {
        private string msg;
        public NotMarkedAsFavorite()
        {
            msg = "Not Marked as Favorite";
        }

        public NotMarkedAsFavorite(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}