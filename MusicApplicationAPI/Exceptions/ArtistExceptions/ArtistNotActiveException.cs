using MusicApplicationAPI.Models.DbModels;
using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.ArtistExceptions
{
    [Serializable]
    public class ArtistNotActiveException : Exception
    {
        private string msg;
        public ArtistNotActiveException()
        {
            msg = "Artist is not active.";
        }

        public ArtistNotActiveException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}