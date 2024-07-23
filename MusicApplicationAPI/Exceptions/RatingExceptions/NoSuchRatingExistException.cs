namespace MusicApplicationAPI.Exceptions.RatingExceptions
{
    [Serializable]
    public class NoSuchRatingExistException : Exception
    {
        private string msg;
        public NoSuchRatingExistException()
        {
            msg = "Rating does not exist.";
        }

        public NoSuchRatingExistException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
