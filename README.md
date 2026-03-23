# Auth.Common.Lib

A .NET library for generating, validating, and configuring JWT authentication, supporting custom roles and easy integration into ASP.NET applications.

##
- Backward compatibility with .Net versions 6, 8, 9, and 10.

## Nuget Download

- Version 1.0.5 has a new method that generates dynamic claims for the token when a level 1 object is provided.

```
dotnet add package Auth.Common.Lib --version 1.0.5
```

## Package Reference
```
<PackageReference Include="Auth.Common.Lib" Version="1.0.5" />
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

### Static Class: `Token`

- `string GenerateToken(CustomToken customToken)`: Generates a JWT token with the provided data.
- `bool TokenValidate(string token)`: Validates a JWT token according to environment settings.

### Static Class: `JwtAuthSettings`

- `void AddJwtAuthSettings(this IServiceCollection services)`: Extension to configure JWT authentication in the ASP.NET Core pipeline.

## Usage Example

```csharp
// Token generation custom object

var customToken = new CustomToken
{
    Email = "user@example.com",
    Roles = "Admin",
    Channel = "99",
    ExpiryTimeInMinutes = 180,
    Cnpj = "12345678901234"
};

or

dynamic customTokenDynamic = new ExpandoObject
        {
            Email = "user@example.com",
            Roles = "Admin",
            Channel = "99",
            ExpiryTimeInMinutes = 180,
            Cnpj = "12345678901234"
        };

var customToken = Token.GenerateCustomToken(customTokenDynamic);
var token = Token.GenerateToken(customToken);

// Token validation
bool isValid = Token.GenerateCustomToken(token);
bool isValid = Token.TokenValidate(token);
```

### ASP.NET Core Configuration

For Swagger to validate your token, you can implement this in your projects that will receive the token for validation.
In your `Startup.cs` or `Program.cs` project builder:

```csharp
services.AddJwtAuthSettings();
```

## Requirements

- .NET 10, 9, 8, or 6
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens
