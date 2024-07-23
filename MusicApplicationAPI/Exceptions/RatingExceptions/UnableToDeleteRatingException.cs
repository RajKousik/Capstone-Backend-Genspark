namespace MusicApplicationAPI.Exceptions.RatingExceptions
{
    [Serializable]
    public class UnableToDeleteRatingException : Exception
    {
        private string msg;
        public UnableToDeleteRatingException()
        {
            msg = "Something went wrong while deleting the rating.";
        }

        public UnableToDeleteRatingException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
