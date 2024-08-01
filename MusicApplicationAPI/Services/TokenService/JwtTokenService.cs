using Microsoft.IdentityModel.Tokens;
using MusicApplicationAPI.Interfaces.Service.TokenService;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicApplicationAPI.Services.TokenService
{
    public class JwtTokenService : ITokenService
    {
        #region Private Fields

        private readonly string _secretKey;
        private readonly SymmetricSecurityKey _key;
        private readonly ILogger<JwtTokenService> _logger;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing the JWT secret key.</param>
        public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
        {
            _secretKey = configuration.GetSection("TokenKey").GetSection("JWT").Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            _logger = logger;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a JWT token based on the provided claims.
        /// </summary>
        /// <param name="claims">The claims to include in the token.</param>
        /// <returns>A JWT token as a string.</returns>

        private string CreateToken(IEnumerable<Claim> claims, int expiryMinutes)
        {
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                expires: DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Public Methods 

        /// <summary>
        /// Generates a JWT token for a user.
        /// </summary>
        /// <param name="user">The user member for whom to generate the token.</param>
        /// <returns>A JWT token as a string.</returns>
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.Username)
            };

            return CreateToken(claims, 360);
        }

        public string GenerateArtistToken(Artist artist)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, artist.ArtistId.ToString()),
                new Claim(ClaimTypes.Role, artist.Role.ToString()),
                new Claim(ClaimTypes.Email, artist.Email),
                new Claim("FullName", artist.Name)
            };

            return CreateToken(claims, 360);
        }

        public string GenerateShortLivedToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.Username),
                new Claim("IsPremiumExpired", "true")
            };

            return CreateToken(claims, 5); // Short-lived token with 5 minutes expiry
        }

        #endregion
    }
}
