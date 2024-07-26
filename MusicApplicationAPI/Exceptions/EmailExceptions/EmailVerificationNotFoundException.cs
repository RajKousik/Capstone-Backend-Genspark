using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.EmailExceptions
{
    [Serializable]
    public class EmailVerificationNotFoundException : Exception
    {
        private string msg;
        public EmailVerificationNotFoundException()
        {
            msg = "Email Verification Not Found";
        }

        public EmailVerificationNotFoundException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}