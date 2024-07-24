using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.RatingExceptions
{
    [Serializable]
    public class UnableToRetrieveTopRatedSongsException : Exception
    {
        private string msg;

        public UnableToRetrieveTopRatedSongsException()
        {
            msg = "Unable To Retrieve The Top Rated Songs.";

        }

        public UnableToRetrieveTopRatedSongsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}