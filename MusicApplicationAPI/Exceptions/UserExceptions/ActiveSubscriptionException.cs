using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.UserExceptions
{
    [Serializable]
    public class ActiveSubscriptionException : Exception
    {
        private string msg;
        public ActiveSubscriptionException()
        {
            msg = "The user still has an active premium subscription.";
        }

        public ActiveSubscriptionException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}