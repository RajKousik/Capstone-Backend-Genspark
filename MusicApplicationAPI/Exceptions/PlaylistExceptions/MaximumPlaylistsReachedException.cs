using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.PlaylistExceptions
{
    [Serializable]
    public class MaximumPlaylistsReachedException : Exception
    {
        private string msg;

        public MaximumPlaylistsReachedException()
        {
            msg = "Normal user has reached the maximum number of playlists.";
        }

        public MaximumPlaylistsReachedException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}