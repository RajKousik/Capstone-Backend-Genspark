using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.SongExceptions
{
    [Serializable]
    public class InvalidSongDuration : Exception
    {
        private string msg;

        public InvalidSongDuration()
        {
            msg = "Invalid song duration";
        }

        public InvalidSongDuration(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}