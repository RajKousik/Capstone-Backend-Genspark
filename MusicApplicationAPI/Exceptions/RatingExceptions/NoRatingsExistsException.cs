namespace MusicApplicationAPI.Exceptions.RatingExceptions
{
    [Serializable]
    public class NoRatingsExistsException : Exception
    {
        private string msg;
        public NoRatingsExistsException()
        {
            msg = "No ratings exist in the database.";
        }

        public NoRatingsExistsException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
