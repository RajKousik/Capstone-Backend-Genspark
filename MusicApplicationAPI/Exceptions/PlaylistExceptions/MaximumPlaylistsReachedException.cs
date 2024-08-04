using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.PlaylistExceptions
{
    [Serializable]
    public class MaximumPlaylistsReachedException : Exception
    {
        private string msg;

        public MaximumPlaylistsReachedException()
        {
            msg = "Maximum Number of playlists reached! Upgrade to premium!";
        }

        public MaximumPlaylistsReachedException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}