using System.Globalization;
using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.EmailExceptions
{
    [Serializable]
    public class InvalidEmailVerificationCodeException : Exception
    {
        private string msg;
        public InvalidEmailVerificationCodeException()
        {
            msg = "Invalid Email Verirication Code";
        }

        public InvalidEmailVerificationCodeException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}