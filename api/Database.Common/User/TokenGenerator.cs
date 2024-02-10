using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database.Common.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace Database.Common.User;

public class TokenGenerator
{
    private readonly string _audience;
    private readonly string _issuer;
    private readonly int _sessionDays;
    private readonly string _signingKey;

    public TokenGenerator(string signingKey)
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
}