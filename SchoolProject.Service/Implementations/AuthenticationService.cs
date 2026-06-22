using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrustructure.IRepositories;
using SchoolProject.Service.Abstracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolProject.Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Filde
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        #endregion
        #region Constructor
        public AuthenticationService(JwtSettings jwtSettings, IRefreshTokenRepository refreshTokenRepository, UserManager<User> userManager)
        {
            _jwtSettings = jwtSettings;

            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }


        #endregion
        #region Handel func

        public async Task<JwtAuthResult> GetJwTToken(User user)
        {
            // var jwtToken = GenerateJwtToken(user);
            var (jwtToken, accessToken) = GenerateJwtToken(user);
            var refresh_Token = GetRefreshToken(user.UserName);
            var userrefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate),
                IsUsed = true,
                IsRevoked = false,
                JwtTId = jwtToken.Id,
                RefreshToken = refresh_Token.TokenRefresh,
                Token = accessToken,
                UserId = user.Id
            };
            await _refreshTokenRepository.AddAsync(userrefreshToken);

            var response = new JwtAuthResult();
            response.refreshToken = refresh_Token;
            response.AccessToken = accessToken;
            return response;
        }
        private (JwtSecurityToken, string) GenerateJwtToken(User user)
        {
            var claims = GetClaims(user);

            var jwtToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessToken);
        }
        private RefreshToken GetRefreshToken(string userName)
        {
            var refresh_Token = new RefreshToken
            {
                ExpireAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate),
                UserName = userName,
                TokenRefresh = GenerateRefreshToken()
            };

            return refresh_Token;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>()
                {
                new Claim(nameof(UserClaimModel.UserName),user.UserName),
                new Claim(nameof(UserClaimModel.Email),user.Email),
                new Claim(nameof(UserClaimModel.PhoneNumber),user.PhoneNumber),
                new Claim(nameof(UserClaimModel.Id),user.Id.ToString())
                 };
            return claims;
        }

        public async Task<JwtAuthResult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expirydate, string refreshToken)
        {

            var (jwtSecurityToken, newToken) = GenerateJwtToken(user);
            var response = new JwtAuthResult();
            response.AccessToken = newToken;
            var refreshtokenrsult = new RefreshToken();
            refreshtokenrsult.UserName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName)).Value;
            refreshtokenrsult.TokenRefresh = refreshToken;
            refreshtokenrsult.ExpireAt = (DateTime)expirydate;
            response.refreshToken = refreshtokenrsult;
            return response;

        }
        public JwtSecurityToken ReadJwtToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;

        }

        public async Task<string> ValidateToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime,
            };

            try
            {
                var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);
                if (validator == null)
                {
                    return "InvalidToken";
                }
                return "NotExpired";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
            #endregion
        }

        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                return ("AlgorithmsIsWrong", null);
            }
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return ("Tokenisnotexpired", null);
            }
            var idClaim = jwtToken.Claims
                         .FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id));

            if (idClaim == null)
            {
                return ("ClaimIdNotFound", null);
            }

            if (!int.TryParse(idClaim.Value, out var userId))
            {
                return ("InvalidUserIdClaim", null);
            }
            var userrefreshtoken = await _refreshTokenRepository.GetTableNoTracking().FirstOrDefaultAsync(x => x.Token == accessToken
                                                                                        && x.RefreshToken == refreshToken
                                                                                        && x.UserId == userId);
            if (userrefreshtoken == null)
            {
                return ("refreshTokenisnotFound", null);
            }
            if (userrefreshtoken.ExpiryDate < DateTime.UtcNow)
            {
                userrefreshtoken.IsRevoked = true;
                userrefreshtoken.IsUsed = false;
                await _refreshTokenRepository.UpdateAsync(userrefreshtoken);
                return ("refreshTokenisexpired", null);
            }
            var expirydate = userrefreshtoken.ExpiryDate;
            return (userId.ToString(), expirydate);
        }
    }
}
