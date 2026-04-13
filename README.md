# Auth.Common.Lib

A .NET library for generating, validating, and configuring JWT authentication, supporting custom roles and easy integration into ASP.NET applications.

## Author
- **Name:** André Canuto
- **Profession:** Software engineer specializing in .Net C#.

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
dotnet add package Auth.Common.Lib --version 1.0.9
```

## Package Reference
```
<PackageReference Include="Auth.Common.Lib" Version="1.0.9" />
```

## Features

- **JWT token generation** with custom information (email, roles, channel, CNPJ, expiration time).
- **JWT token validation** according to environment and security parameters.
- **Default roles enumeration** for access control.
- **Simplified JWT configuration** for ASP.NET Core via service extension.

## Installation

Add a reference to the `Auth.Common.Lib` project in your .NET 10, 9, 8, 6 solution.

## Environment Configuration

Set the following environment variables for correct operation:

- `ISSUER`: Token issuer identifier.
- `AUDIENCE`: Token audience.
- `DEFAULTSECRET`: Secret key for signing tokens.

## Main Components

### Enum: `DefaultRoles`

Enumeration of default roles:
- `Visitor`
- `Common`
- `Manager`
- `Admin`

### Class: `CustomToken`

Model for token generation:
- `Email` (required)
- `Roles` (required, e.g., Visitor, Common, Manager, Admin)
- `Channel` (required, e.g., "99")
- `ExpiryTimeInMinutes` (required, default: 180)
- `Cnpj` (optional, default: "DXZN0F5CZD3830")
- ...

### Static Class: `Token`

- `string GenerateToken(CustomToken customToken)`: Generates a JWT token with the provided data.
- `bool TokenValidate(string token)`: Validates a JWT token according to environment settings.

### Static Class: `JwtAuthSettings`

- `void AddJwtAuthSettings(this IServiceCollection services)`: Extension to configure JWT authentication in the ASP.NET Core pipeline.

## Usage Example

```csharp
// Specific object for token generation

var customToken = new CustomToken
{
    Email = "user@example.com",
    Roles = "Admin",
    ExpiryTimeInMinutes = 180,
    Cnpj = "12345678901234"
};

//Or

// Dynamic object for token generation
double expiryTimeInMinutes = 300;
dynamic customTokenDynamic = new ExpandoObject
{
    Email = "user@example.com",
    Roles = "Admin",
    Channel = "99",
    UserId = "123SA4567890HA1234"
};

// Generate token with dynamic object
var customToken = Token.GenerateCustomToken(customTokenDynamic, expiryTimeInMinutes);

// Generate token with specific object
var token = Token.GenerateToken(customToken);

// Token validation
bool isValid = token.IsValidToken(token);
bool isValid = customToken.IsValidToken(token);
```

### ASP.NET Core Configuration

For Swagger to validate your token, you can implement this in your projects that will receive the token for validation.
In your `Startup.cs` or `Program.cs` project builder:

```csharp
services.AddJwtAuthSettings();
```

### Do not forget to inform the environment variables
Example informing the environment variable in launchSettings.json
```
"DEFAULTSECRET": "09832ho23h09r32hre...",
"ISSUER": "Canuto",
"AUDIENCE": "canuto-api"
```

## Requirements

- .NET 10, 9, 8, or 6
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens