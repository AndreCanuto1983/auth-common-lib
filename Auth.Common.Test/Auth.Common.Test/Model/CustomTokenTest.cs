using Auth.Common.Lib.Model;
using System.ComponentModel.DataAnnotations;

namespace Auth.Common.Lib.Test.Model
{
    public class CustomTokenTest
    {
        [Fact]
        public void CustomToken_Properties_AssignAndRetrieve()
        {
            var token = new CustomToken
            {
                Email = "user@example.com",
                Roles = "Admin",
                Channel = "99",
                ExpiryTimeInMinutes = 120,
                Cnpj = "12345678901234"
            };

            Assert.Equal("user@example.com", token.Email);
            Assert.Equal("Admin", token.Roles);
            Assert.Equal("99", token.Channel);
            Assert.Equal(120, token.ExpiryTimeInMinutes);
            Assert.Equal("12345678901234", token.Cnpj);
        }

        [Fact]
        public void CustomToken_Validation_MissingRequiredFields_ShouldFail()
        {
            var token = new CustomToken();
            var context = new ValidationContext(token);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(token, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Email is required"));
            Assert.Contains(results, r => r.ErrorMessage.Contains("Roles is required"));
            Assert.Contains(results, r => r.ErrorMessage.Contains("Channel is required"));
        }

        [Fact]
        public void CustomToken_Validation_ValidObject_ShouldPass()
        {
            var token = new CustomToken
            {
                Email = "user@example.com",
                Roles = "Manager",
                Channel = "99",
                ExpiryTimeInMinutes = 180,
                Cnpj = "DXZN0F5CZD3830"
            };
            var context = new ValidationContext(token);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(token, context, results, true);

            Assert.True(isValid);
            Assert.Empty(results);
        }
    }
}
