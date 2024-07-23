namespace MusicApplicationAPI.Exceptions.UserExceptions
{
    [Serializable]
    public class UnableToAddUserException : Exception
    {
        private string msg;
        public UnableToAddUserException()
        {
            msg = "Unable to add a user to the Database";
        }

        public UnableToAddUserException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
