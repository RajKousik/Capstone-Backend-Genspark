using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.RatingExceptions
{
    [Serializable]
    public class AlreadyRatedException : Exception
    {
        private string msg;

        public AlreadyRatedException()
        {
            msg = "User has already rated this song.";
        }

        public AlreadyRatedException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}