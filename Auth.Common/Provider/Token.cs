using Auth.Common.Lib.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Common.Lib.Provider
{
    public static class Token
    {
        /*
         * Generate the token through dynamic data from an object, example: { Name = "Andrew Canuto", Role = "Admin" }
         * If the token expiration time is not specified, it will start with 120 minutes, which is equivalent to 2 hours
         */
        public static string GenerateCustomToken(dynamic customData, double expiryTimeInMinutes = 120)
        {
            var issuer = Environment.GetEnvironmentVariable("ISSUER");
            var audience = Environment.GetEnvironmentVariable("AUDIENCE");
            var secret = Environment.GetEnvironmentVariable("DEFAULTSECRET");

            var claims = new List<Claim>();

            foreach (PropertyInfo prop in customData.GetType().GetProperties())
            {
                var value = prop.GetValue(customData, null);
                if (value != null)
                {
                    claims.Add(new Claim(prop.Name, value.ToString()));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(expiryTimeInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    SecurityAlgorithms.HmacSha512Signature),
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /*
         * Generate a token through a specific object
         */
        public static string GenerateToken(this CustomToken customToken)
        {
            var issuer = Environment.GetEnvironmentVariable("ISSUER");
            var audience = Environment.GetEnvironmentVariable("AUDIENCE");
            var secret = Environment.GetEnvironmentVariable("DEFAULTSECRET");

            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(customToken?.Name))
                claims.Add(new Claim(ClaimTypes.Name, customToken.Name));

            if (!string.IsNullOrEmpty(customToken?.Email))
                claims.Add(new Claim(ClaimTypes.Email, customToken.Email));

            if (!string.IsNullOrEmpty(customToken?.Roles))
                claims.Add(new Claim(ClaimTypes.Role, customToken.Roles));

            if (customToken?.UserId != null)
                claims.Add(new Claim("UserId", customToken.UserId.ToString()));

            if (customToken?.CustomerId != null)
                claims.Add(new Claim("CustomerId", customToken.CustomerId.ToString()));

            if (!string.IsNullOrEmpty(customToken?.AccountStatus))
                claims.Add(new Claim("AccountStatus", customToken.AccountStatus));

            if (!string.IsNullOrEmpty(customToken?.Cnpj))
                claims.Add(new Claim("Cnpj", customToken.Cnpj));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(customToken?.ExpiryTimeInMinutes ?? 60),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    SecurityAlgorithms.HmacSha512Signature),
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /*
         * Check if a token is valid
         */
        public static bool IsValidToken(this string token)
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
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        /*
         * Get all token claims
         */
        public static dynamic ReadToken(this string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                IDictionary<string, object> claimsDict = new Dictionary<string, object>();
                foreach (var claim in jwtToken.Claims)
                {
                    claimsDict[claim.Type] = claim.Value;
                }

                return claimsDict;
            }
            catch
            {
                return string.Empty;
            }
        }

        /*
         * Dynamic secrets generator for use in APIs
         */
        public static string GenerateSecret(int size = 64)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(size);
            return Convert.ToBase64String(randomBytes);
        }
    }
}