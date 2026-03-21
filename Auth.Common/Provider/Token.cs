using Auth.Common.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Common.Provider
{
    public static class Token
    {
        public static string GenerateToken(CustomToken customToken)
        {
            var issuer = Environment.GetEnvironmentVariable("ISSUER");
            var audience = Environment.GetEnvironmentVariable("AUDIENCE");
            var secret = Environment.GetEnvironmentVariable("DEFAULTSECRET");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, customToken.Email),
                    new(ClaimTypes.Role, customToken.Roles),
                    new("Channel", customToken.Channel),
                    new("Cnpj", customToken.Cnpj)
                }),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(customToken.ExpiryTimeInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    SecurityAlgorithms.HmacSha256Signature),
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static bool TokenValidate(string token)
        {
            try
            {
                ///Ways to read token
                ///var tokenTest = tokenHandler.ReadJwtToken(token);                     
                ///var tokenTest1 = tokenHandler.ReadToken(token);  
                var issuer = Environment.GetEnvironmentVariable("ISSUER");
                var audience = Environment.GetEnvironmentVariable("AUDIENCE");
                var secret = Environment.GetEnvironmentVariable("DEFAULTSECRET");

                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true, ///enable key validation
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), ///key from line 11
                    ValidateIssuer = true, ///enable issuer validation
                    ValidIssuer = issuer, //validate issuer
                    ValidateAudience = true, ///enable token origin validation
                    ValidAudience = audience, ///validate token origin (url). I can pass a collection if I want
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true //validate token signature, basically the SigningCredentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                ///ValidateToken returns the main token data if you want to reuse it
                tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
