# Auth.Common.Lib

A .NET library for generating, validating, and configuring JWT authentication, supporting custom roles and easy integration into ASP.NET applications.

## Features

- **JWT token generation** with custom information (email, roles, channel, CNPJ, expiration time).
- **JWT token validation** according to environment and security parameters.
- **Default roles enumeration** for access control.
- **Simplified JWT configuration** for ASP.NET Core via service extension.

## Installation

Add a reference to the `Auth.Common` project in your .NET 10 solution.

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
// Token generation
var customToken = new CustomToken
{
    Email = "user@example.com",
    Roles = "Admin",
    Channel = "99",
    ExpiryTimeInMinutes = 180,
    Cnpj = "12345678901234"
};

string token = Token.GenerateToken(customToken);

// Token validation
bool isValid = Token.TokenValidate(token);
```

### ASP.NET Core Configuration

In your `Startup.cs` or project builder:

```csharp
services.AddJwtAuthSettings();
```

## Requirements

- .NET 10
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens
