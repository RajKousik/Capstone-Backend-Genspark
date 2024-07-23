namespace MusicApplicationAPI.Exceptions.UserExceptions
{
    [Serializable]
    public class UnauthorizedUserException : Exception
    {
        private string msg;
        public UnauthorizedUserException()
        {
            msg = "Invalid email or Password";
        }

        public UnauthorizedUserException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
