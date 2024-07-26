using System.Runtime.Serialization;

namespace MusicApplicationAPI.Services.UserService
{
    [Serializable]
    public class NoSuchPremiumUserExistException : Exception
    {
        private string msg;
        public NoSuchPremiumUserExistException()
        {
            msg = "The user is not a premium user.";
        }

        public NoSuchPremiumUserExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}