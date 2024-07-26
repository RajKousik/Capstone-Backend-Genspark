using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Easy_Password_Validator;
using MusicApplicationAPI.Exceptions.UserExceptions;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Interfaces.Service.AuthService;
using MusicApplicationAPI.Models.DTOs.UserDTO;

namespace MusicApplicationAPI.Services.UserService
{
    public class PasswordService : IPasswordService
    {

        #region Private Fields
        private readonly PasswordValidatorService _passwordValidatorService;
        private readonly bool AllowPasswordValidation = false;
        #endregion

        #region Constructor
        public PasswordService(PasswordValidatorService passwordValidatorService,
            IConfiguration configuration)
        {
            _passwordValidatorService = passwordValidatorService;
            bool.TryParse(configuration.GetSection("AllowPasswordValidation").Value, out bool allowPasswordValidation);
            AllowPasswordValidation = allowPasswordValidation;
        }
        #endregion

        public byte[] HashPassword(string password, out byte[] key)
        {
            #region Password Validation
            if (AllowPasswordValidation)
            {
                var isPasswordValid = _passwordValidatorService.TestAndScore(password);
                Debug.WriteLine($"Password score {_passwordValidatorService.Score}, {_passwordValidatorService.FailureMessages} ");
                var failureMessages = string.Join(", ", _passwordValidatorService.FailureMessages);
                if (!isPasswordValid)
                {
                    Debug.WriteLine("Invalid password");
                    throw new InvalidPasswordException(failureMessages);
                }
            }
            #endregion
            using (var hmac = new HMACSHA512())
            {
                key = hmac.Key;
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return hash;
            }
        }

        public bool VerifyPassword(string password, byte[] passwordHash, byte[] key)
        {
            using (var hmac = new HMACSHA512(key))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
