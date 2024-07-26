using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.CommonExceptions
{
    [Serializable]
    public class UnableToSendMailException : Exception
    {
        private string msg;
        public UnableToSendMailException()
        {
            msg = "Unable to send the Mail as of now!";
        }

        public UnableToSendMailException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}