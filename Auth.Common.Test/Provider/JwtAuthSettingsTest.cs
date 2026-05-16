using Auth.Common.Lib.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Common.Lib.Test.Provider
{
    public class JwtAuthSettingsTest
    {
        [Fact]
        public void AddJwtAuthSettings_RegistersJwtBearerAuthentication()
        {
            // Arrange
            var services = new ServiceCollection();
            Environment.SetEnvironmentVariable("ISSUER", "test-issuer");
            Environment.SetEnvironmentVariable("AUDIENCE", "test-audience");
            Environment.SetEnvironmentVariable("DEFAULTSECRET", "supersecretkey1234567890");

            // Act
            services.AddJwtAuthSettings();
            var provider = services.BuildServiceProvider();
            var authService = provider.GetService<Microsoft.AspNetCore.Authentication.IAuthenticationService>();

            // Assert
            Assert.NotNull(authService);
        }
    }
}
