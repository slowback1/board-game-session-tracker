using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database.Common.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace Database.Common.User;

public class TokenProvider
{
    private readonly string _audience;
    private readonly string _issuer;
    private readonly int _sessionDays;
    private readonly string _signingKey;

    public TokenProvider(string signingKey)
    {
        _signingKey = signingKey;
        _issuer = "";
        _audience = "";
        _sessionDays = 365;
    }

    public string GenerateToken(UserDTO user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_signingKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            new[]
            {
                new Claim("username", user.Username),
                new Claim("userId", user.UserId),
                new Claim("createdAt", user.CreatedAt.ToLongDateString())
            },
            expires: DateTime.Now.AddDays(_sessionDays),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public UserDTO? DecodeToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false
        };

        SecurityToken securityToken = null;
        try
        {
            var claimsPrincipal = new JwtSecurityTokenHandler()
                .ValidateToken(token, tokenValidationParameters, out securityToken);

            var username = claimsPrincipal.Claims.First(c => c.Type == "username").Value;
            var userId = claimsPrincipal.Claims.First(c => c.Type == "userId").Value;
            var createdAt = claimsPrincipal.Claims.First(c => c.Type == "createdAt").Value;

            return new UserDTO
            {
                Username = username,
                CreatedAt = DateTime.Parse(createdAt),
                UserId = userId
            };
        }
        catch
        {
            return null;
        }
    }
}