using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.SongExceptions
{
    [Serializable]
    public class InvalidGenreException : Exception
    {
        private string msg;

        public InvalidGenreException()
        {
            msg = "Not a valid genre";
        }

        public InvalidGenreException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}