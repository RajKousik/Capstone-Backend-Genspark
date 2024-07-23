namespace MusicApplicationAPI.Exceptions.RatingExceptions
{
    [Serializable]
    public class UnableToUpdateRatingException : Exception
    {
        private string msg;
        public UnableToUpdateRatingException()
        {
            msg = "Something went wrong while updating the rating.";
        }

        public UnableToUpdateRatingException(string message) : base(message)
        {
            msg = message;
        }

        public override string Message => msg;
    }
}
