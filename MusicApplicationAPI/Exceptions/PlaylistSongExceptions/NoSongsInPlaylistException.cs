using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.PlaylistSongExceptions
{
    [Serializable]
    public class NoSongsInPlaylistException : Exception
    {
        private string msg;
        public NoSongsInPlaylistException()
        {
            msg = "No songs present in the playlist";
        }

        public NoSongsInPlaylistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;


    }
}