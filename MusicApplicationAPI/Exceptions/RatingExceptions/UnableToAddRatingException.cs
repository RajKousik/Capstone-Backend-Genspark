namespace MusicApplicationAPI.Exceptions.RatingExceptions
{
    [Serializable]
    public class UnableToAddRatingException : Exception
    {
        private string msg;
        public UnableToAddRatingException()
        {
            msg = "Something went wrong while adding the rating.";
        }

        public UnableToAddRatingException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
