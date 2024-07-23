namespace MusicApplicationAPI.Exceptions.UserExceptions
{
    [Serializable]
    public class NoSuchUserExistException : Exception
    {
        private string msg;
        public NoSuchUserExistException()
        {
            msg = "No Such User Found in the Database";
        }

        public NoSuchUserExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;

    }
}
