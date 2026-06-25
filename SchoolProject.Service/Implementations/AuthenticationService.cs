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
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthenticationService(
            JwtSettings jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
            UserManager<User> userManager)
        {
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }

        #region JWT Generation

        public async Task<JwtAuthResult> GetJwTToken(User user)
        {
            var (jwtToken, accessToken) = await GenerateJwtToken(user);
            var refreshToken = GetRefreshToken(user);

            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.UtcNow,
                ExpiryDate = refreshToken.ExpireAt,
                IsUsed = false,
                IsRevoked = false,
                JwtTId = jwtToken.Id,
                RefreshToken = refreshToken.TokenRefresh,
                Token = accessToken,
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(userRefreshToken);

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                refreshToken = refreshToken
            };
        }

        private async Task<(JwtSecurityToken token, string accessToken)> GenerateJwtToken(User user)
        {
            var claims = await GetClaims(user);

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return (jwtToken, accessToken);
        }

        #endregion

        #region Claims

        public async Task<List<Claim>> GetClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
           {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
           };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            return claims;
        }

        #endregion

        #region Refresh Token

        private RefreshToken GetRefreshToken(User user)
        {
            return new RefreshToken
            {
                UserName = user.UserName,
                TokenRefresh = GenerateRefreshToken(),
                ExpireAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate)
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public async Task<JwtAuthResult> GetRefreshToken(
            User user,
            JwtSecurityToken jwtToken,
            DateTime? expiryDate,
            string refreshToken)
        {
            var (_, newAccessToken) = await GenerateJwtToken(user);

            var refresh = new RefreshToken
            {
                UserName = user.UserName,
                TokenRefresh = refreshToken,
                ExpireAt = expiryDate ?? DateTime.UtcNow
            };

            return new JwtAuthResult
            {
                AccessToken = newAccessToken,
                refreshToken = refresh
            };
        }

        #endregion

        #region Read Token

        public JwtSecurityToken ReadJwtToken(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentNullException(nameof(accessToken));

            return new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        }

        #endregion

        #region Validate Token

        public async Task<string> ValidateToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuer = _jwtSettings.Issuer,

                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidAudience = _jwtSettings.Audience,

                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);
                return "NotExpired";
            }
            catch
            {
                return "InvalidToken";
            }
        }

        #endregion

        #region Validate Details (FIXED COMPLETELY)

        public async Task<(string, DateTime?)> ValidateDetails(
            JwtSecurityToken jwtToken,
            string accessToken,
            string refreshToken)
        {
            if (jwtToken == null)
                return ("InvalidToken", null);

            // IMPORTANT FIX: correct algorithm check
            if (!jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return ("AlgorithmsIsWrong", null);
            }

            // token still valid
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return ("Tokenisnotexpired", null);
            }

            // safe claim extraction
            var idClaim = jwtToken.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idClaim))
                return ("ClaimIdNotFound", null);

            if (!int.TryParse(idClaim, out var userId))
                return ("InvalidUserIdClaim", null);

            var storedToken = await _refreshTokenRepository
                .GetTableNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.RefreshToken == refreshToken &&
                    x.UserId == userId);

            if (storedToken == null)
                return ("refreshTokenisnotFound", null);

            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;

                await _refreshTokenRepository.UpdateAsync(storedToken);

                return ("refreshTokenisexpired", null);
            }

            return (userId.ToString(), storedToken.ExpiryDate);
        }

        #endregion
    }
}
