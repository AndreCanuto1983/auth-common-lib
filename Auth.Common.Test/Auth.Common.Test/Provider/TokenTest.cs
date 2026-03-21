using Auth.Common.Lib.Model;
using Auth.Common.Lib.Provider;

namespace Auth.Common.Lib.Test.Provider
{
    public class TokenTest
    {
        [Fact]
        public void GenerateToken_ReturnsValidJwtToken()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ISSUER", "test-issuer");
            Environment.SetEnvironmentVariable("AUDIENCE", "test-audience");
            Environment.SetEnvironmentVariable("DEFAULTSECRET", "supersecretkey12345678901234567890");

            var customToken = new CustomToken
            {
                Email = "user@example.com",
                Roles = "Admin",
                Channel = "99",
                ExpiryTimeInMinutes = 60,
                Cnpj = "12345678901234"
            };

            // Act
            var jwt = Token.GenerateToken(customToken);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(jwt));
        }

        [Fact]
        public void TokenValidate_ValidToken_ReturnsTrue()
        {
            // Arrange
            var prevIssuer = Environment.GetEnvironmentVariable("ISSUER");
            try
            {
                Environment.SetEnvironmentVariable("ISSUER", "test-issuer");
                Environment.SetEnvironmentVariable("AUDIENCE", "test-audience");
                Environment.SetEnvironmentVariable("DEFAULTSECRET", "supersecretkey12345678901234567890");

                var customToken = new CustomToken
                {
                    Email = "user@example.com",
                    Roles = "Admin",
                    Channel = "99",
                    ExpiryTimeInMinutes = 60,
                    Cnpj = "12345678901234"
                };
                var jwt = Token.GenerateToken(customToken);

                // Act
                var isValid = Token.TokenValidate(jwt);

                // Assert
                Assert.True(isValid);
            }
            finally
            {
                Environment.SetEnvironmentVariable("ISSUER", prevIssuer);
            }
        }

        [Fact]
        public void TokenValidate_InvalidToken_ReturnsFalse()
        {
            var prevIssuer = Environment.GetEnvironmentVariable("ISSUER");
            try
            {
                Environment.SetEnvironmentVariable("ISSUER", "test-issuer");
                Environment.SetEnvironmentVariable("AUDIENCE", "test-audience");
                Environment.SetEnvironmentVariable("DEFAULTSECRET", "supersecretkey12345678901234567890");

                var jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30";

                var isValid = Token.TokenValidate(jwt);

                Assert.False(isValid);
            }
            finally
            {
                Environment.SetEnvironmentVariable("ISSUER", prevIssuer);
            }
        }
    }
}
