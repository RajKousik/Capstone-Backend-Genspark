using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.EmailExceptions
{
    [Serializable]
    public class VerificationExpiredException : Exception
    {
        private string msg;
        public VerificationExpiredException()
        {
            msg = "Verifiation Code Expired";
        }

        public VerificationExpiredException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}