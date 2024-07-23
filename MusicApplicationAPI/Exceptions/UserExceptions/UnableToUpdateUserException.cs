namespace MusicApplicationAPI.Exceptions.UserExceptions
{
    [Serializable]
    public class UnableToUpdateUserException : Exception
    {
        private string msg;
        public UnableToUpdateUserException()
        {
            msg = "Something went wrong while updating a user record";
        }

        public UnableToUpdateUserException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
