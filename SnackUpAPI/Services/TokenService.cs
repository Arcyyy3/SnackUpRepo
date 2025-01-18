using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SnackUpAPI.Services
{
    public class TokenService
    {
        private readonly string _jwtSecretKey;
        private readonly int _tokenExpiryInMinutes;

        public TokenService(string jwtSecretKey, int tokenExpiryInMinutes = 60)
        {
            _jwtSecretKey = jwtSecretKey ?? throw new ArgumentNullException(nameof(jwtSecretKey));
            _tokenExpiryInMinutes = tokenExpiryInMinutes;
        }

        // Genera un access token JWT
        public string GenerateToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpiryInMinutes),
                Issuer = "SnackUpAPI",
                Audience = "SnackUpClients",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
      

        // Estrae l'ID utente da un token JWT scaduto
        public int? GetUserIdFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecretKey);

            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false, // Consente di leggere token scaduti
                    ValidIssuer = "SnackUpAPI",
                    ValidAudience = "SnackUpClients",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out var securityToken);

                if (securityToken is JwtSecurityToken jwtSecurityToken &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userIdClaim = principal.FindFirst("UserId");
                    return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
                }
            }
            catch
            {
                // Ignora errori durante la validazione del token
            }

            return null;
        }
    }
}
