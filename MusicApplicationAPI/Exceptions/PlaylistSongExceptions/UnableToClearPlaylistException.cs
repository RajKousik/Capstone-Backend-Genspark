using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.PlaylistSongExceptions
{
    [Serializable]
    public class UnableToClearPlaylistException : Exception
    {
        private string msg;
        public UnableToClearPlaylistException()
        {
            msg = "Something went wrong while clearing the songs in the playlist";
        }

        public UnableToClearPlaylistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;

    }
}