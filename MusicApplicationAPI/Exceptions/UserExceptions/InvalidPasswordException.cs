﻿namespace MusicApplicationAPI.Exceptions.UserExceptions
{
    [Serializable]
    public class InvalidPasswordException : Exception
    {
        private string message;
        public InvalidPasswordException()
        {
            message = "Your password should contain at least one lowercase letter, one uppercase letter, one number,  one special character and also a minimum of 8 characters in length.";
        }

        public InvalidPasswordException(string message)
        {
            this.message = message;
        }

        public override string Message => message;
    }
}
