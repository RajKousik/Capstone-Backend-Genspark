namespace MusicApplicationAPI.Exceptions.UserExceptions
{
    [Serializable]
    public class UnableToDeleteUserException : Exception
    {
        private string msg;
        public UnableToDeleteUserException()
        {
            msg = "Something went wrong while deleting a user record";
        }

        public UnableToDeleteUserException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
