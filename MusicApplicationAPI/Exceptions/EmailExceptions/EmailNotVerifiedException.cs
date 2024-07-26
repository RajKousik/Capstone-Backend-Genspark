using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.EmailExceptions
{
    [Serializable]
    public class EmailNotVerifiedException : Exception
    {
        private string msg;
        public EmailNotVerifiedException()
        {
            msg = "Your Email is Not Verified";
        }

        public EmailNotVerifiedException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}