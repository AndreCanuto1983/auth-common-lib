using Auth.Common.Lib.Model;
using Auth.Common.Lib.Provider;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Common.Lib.Test.Provider
{
    public class TokenTest
    {
        [Fact]
        public void GenerateCustomToken_DeveGerarTokenComClaimsEsperados()
        {
            // Arrange
            dynamic customData = new
            {
                UserId = 123,
                CustomerId = 456,
                AccountStatus = "Active",
                Cnpj = "12345678000199"
            };

            // Configura variáveis de ambiente usadas pelo método
            Environment.SetEnvironmentVariable("ISSUER", "TestIssuer");
            Environment.SetEnvironmentVariable("AUDIENCE", "TestAudience");
            Environment.SetEnvironmentVariable("DEFAULTSECRET", "segredo_super_secreto_para_testes");

            // Act
            string token = Token.GenerateCustomToken(customData);
            string token2 = Token.GenerateCustomToken(customData, 300);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var jwtToken2 = handler.ReadJwtToken(token2);

            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Equal("TestAudience", jwtToken.Audiences.First());

            // Verifica se os claims foram adicionados corretamente
            Assert.Contains(jwtToken.Claims, c => c.Type == "UserId" && c.Value == "123");
            Assert.Contains(jwtToken.Claims, c => c.Type == "CustomerId" && c.Value == "456");
            Assert.Contains(jwtToken.Claims, c => c.Type == "AccountStatus" && c.Value == "Active");
            Assert.Contains(jwtToken.Claims, c => c.Type == "Cnpj" && c.Value == "12345678000199");

            // Verifica se a expiração está dentro do intervalo esperado
            Assert.True(jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(120));
            Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
            Assert.True(jwtToken2.ValidTo > DateTime.UtcNow);
        }

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
                Name = "Canuto",
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
                    UserId = "99",
                    ExpiryTimeInMinutes = 60,
                    Cnpj = "12345678901234"
                };
                var jwt = Token.GenerateToken(customToken);

                // Act
                var isValid = Token.IsValidToken(jwt);

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

                var isValid = Token.IsValidToken(jwt);

                Assert.False(isValid);
            }
            finally
            {
                Environment.SetEnvironmentVariable("ISSUER", prevIssuer);
            }
        }

        [Fact]
        public void ReadToken_ShouldReturnClaimsDictionary_WhenTokenIsValid()
        {
            // Arrange: cria um token de teste
            var secret = "supersecretkey1234567890lkasdflkaskdasl";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim("UserId", "123"),
            new Claim("Email", "user@example.com"),
            new Claim("Role", "Admin")
        };

            var token = new JwtSecurityToken(
                issuer: "TestIssuer",
                audience: "TestAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Act: chama sua função
            var result = tokenString.ReadToken();

            // Assert: verifica se os claims foram retornados corretamente
            Assert.NotNull(result);
            Assert.Equal("123", result["UserId"]);
            Assert.Equal("user@example.com", result["Email"]);
            Assert.Equal("Admin", result["Role"]);
        }

        [Fact]
        public void ReadToken_ShouldReturnEmptyString_WhenTokenIsInvalid()
        {
            // Arrange: token inválido
            var invalidToken = "abc.def.ghi";

            // Act
            var result = invalidToken.ReadToken();

            // Assert
            Assert.Equal(string.Empty, result);

        }
    }
}