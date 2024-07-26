using System.Runtime.Serialization;

namespace MusicApplicationAPI.Exceptions.UserExceptions
{
    [Serializable]
    public class PremiumSubscriptionExpiredException : Exception
    {
        private string msg;
        public PremiumSubscriptionExpiredException()
        {
            msg = "Your premium subscription has expired. Please renew to continue enjoying premium features.";
        }

        public PremiumSubscriptionExpiredException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}