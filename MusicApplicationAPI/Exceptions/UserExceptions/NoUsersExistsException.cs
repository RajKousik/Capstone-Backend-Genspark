namespace MusicApplicationAPI.Exceptions.UserExceptions
{
    [Serializable]
    public class NoUsersExistsExistsException : Exception
    {
        private string msg;
        public NoUsersExistsExistsException()
        {
            msg = "No users Found in the database";
        }
        public NoUsersExistsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;


    }
}
