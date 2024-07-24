using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.FavoriteExceptions
{
    [Serializable]
    public class AlreadyMarkedAsFavorite : Exception
    {
        private string msg;
        public AlreadyMarkedAsFavorite()
        {
            msg = "Already marked as favorite";
        }

        public AlreadyMarkedAsFavorite(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}