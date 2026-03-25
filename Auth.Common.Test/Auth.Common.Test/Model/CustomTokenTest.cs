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
                Name = "Andrew Canuto",
                ExpiryTimeInMinutes = 120,
                Cnpj = "12345678901234",
                AccountStatus = "active",
                CustomerId = 12345,
                UserId = "67890"
            };

            Assert.Equal("user@example.com", token.Email);
            Assert.Equal("Admin", token.Roles);
            Assert.Equal("Andrew Canuto", token.Name);
            Assert.Equal(120, token.ExpiryTimeInMinutes);
            Assert.Equal("12345678901234", token.Cnpj);
            Assert.Equal("active", token.AccountStatus);
            Assert.Equal(12345, long.Parse(token.CustomerId.ToString()));
            Assert.Equal("67890", token.UserId.ToString());
        }

        [Fact]
        public void CustomToken_Validation_ReturnTrue()
        {
            var token = new CustomToken();
            var context = new ValidationContext(token);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(token, context, results, true);

            Assert.True(isValid);
        }

        [Fact]
        public void CustomToken_Validation_ValidObject_ShouldPass()
        {
            var token = new CustomToken
            {
                Email = "user@example.com",
                Roles = "Manager",
                Name = "Andrew Canuto",
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
