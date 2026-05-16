# Auth.Common.Lib

A .NET library for generating, validating, and configuring JWT authentication, supporting custom roles and easy integration into ASP.NET applications.

## Author
- **Name:** André Canuto
- **Profession:** Software engineer specializing in .Net C#.

## Features
- **JWT token generation** with custom information (email, roles, channel, CNPJ, expiration time).
- **JWT token validation** according to environment and security parameters.
- **Default roles enumeration** for access control.
- **Simplified JWT configuration** for ASP.NET Core via service extension.
  
# Improvements in version 1.0.10
- The fixed model and method for creating the token have been discontinued. Now, only the dynamic method is used, where any desired parameter can be added to the token.
- Maintains compatibility with the previous version.

- The dependencies below have been updated for .Net version 10.
<PackageReference Include="Microsoft.IdentityModel.Tokens" Version="8.18.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.18.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.8" />

# Improvements in version 1.0.9
- Backward compatibility with .Net versions 6, 8, 9, and 10.
- Everything from the previous version
- Implemented method to read the token and return the claims.
- Dynamic secrets generator for use in APIs

# Improvements in version 1.0.8
- Improvement in the custom token generation function and adding new parameters in the token object with predefined data
- A new method that generates dynamic claims for the token when a level 1 object is provided.

## Nuget Download

```
dotnet add package Auth.Common.Lib --version 1.0.10
```

## Package Reference
```
<PackageReference Include="Auth.Common.Lib" Version="1.0.10" />
```

## Installation
- Add a reference to the `Auth.Common.Lib` project in your .NET 10, 9, 8, 6 solution.

## Main Components

### Static Class: `Token`
- `string token = Token.GenerateCustomToken(customTokenDynamic, expiryTimeInMinutes)` -> Generates a JWT token with the provided data.
- `bool isValid = customToken.IsValidToken();` -> Validates a JWT token according to environment settings, or use `bool isValid = Token.IsValidToken(token);`.
  
## Environment Configuration

Set the following environment variables for correct operation:
- `ISSUER`: Token issuer identifier.
- `AUDIENCE`: Token audience.
- `DEFAULTSECRET`: Secret key for signing tokens.

- Example informing the environment variable in Properties/launchSettings.json

```

"environmentVariables": {
    "DEFAULTSECRET": "KJ12B3L123LK1LK7...",
    "ISSUER": "Canuto",
    "AUDIENCE": "canuto-api"
}

```

### ASP.NET Core Configuration
- For Swagger to validate your token, you can implement this in your projects that will receive the token for validation.
In your `Program.cs` or `Startup.cs` project builder:

```csharp

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddJwtAuthSettings();

```

## Usage Example

```csharp

// This is a dynamic object for generating tokens; you can add any information, as long as you follow the example below, where you must send the attribute and the value.
dynamic customTokenDynamic = new ExpandoObject
{
    Email = "user@example.com",
    Roles = "Admin",
    Channel = "99",
    UserId = "123SA4567890HA1234",
    CustomerId = "LKASHE9823ODKJ23"
};

// Generate token with dynamic object, if you do not specify the expiryTimeInMinutes, the token expiration time is automatically set to 120 minutes UTC.
double expiryTimeInMinutes = 300;
var customToken = Token.GenerateCustomToken(customTokenDynamic, expiryTimeInMinutes);

// Checks if the token is valid, if necessary.
bool isValid = customToken.IsValidToken(token);

```

## Requirements
- .NET 10, 9, 8, or 6
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens
