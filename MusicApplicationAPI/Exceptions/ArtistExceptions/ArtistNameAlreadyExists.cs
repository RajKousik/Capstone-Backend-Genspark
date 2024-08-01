using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.ArtistExceptions
{
    [Serializable]
    public class ArtistNameAlreadyExists : Exception
    {
        public string msg;
        public ArtistNameAlreadyExists()
        {
            msg = "Artist Name already taken";
        }

        public ArtistNameAlreadyExists(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;

    }
}