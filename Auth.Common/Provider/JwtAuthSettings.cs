using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.Common.Provider
{
    public static class JwtAuthSettings
    {
        public static void AddJwtAuthSettings(this IServiceCollection services)
        {
            var issuer = Environment.GetEnvironmentVariable("ISSUER");
            var audience = Environment.GetEnvironmentVariable("AUDIENCE");
            var secret = Environment.GetEnvironmentVariable("DEFAULTSECRET");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false; //require https
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //enable key validation
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), //secret
                    ValidateIssuer = true, //enable issuer validation
                    ValidIssuer = issuer, //validate the issuer
                    ValidateAudience = true, //enable token origin validation
                    ValidAudience = audience, //validate the token origin (url). You can pass a collection if you want
                    RequireSignedTokens = true //validate token signature, basically the SigningCredentials
                };
            });
        }
    }
}
