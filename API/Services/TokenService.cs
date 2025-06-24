using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"];
        if (string.IsNullOrEmpty(tokenKey))
            throw new InvalidOperationException("Token key is not configured.");

        if (tokenKey.Length < 64)
            throw new InvalidOperationException("Token key need to be longer.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserName),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        if (token == null)
            throw new InvalidOperationException("Failed to create JWT token.");

        return tokenHandler.WriteToken(token);
    }

    public void RevokeAllRefreshTokens(AppUser user)
    {
        throw new NotImplementedException();
    }

    public void RevokeRefreshToken(string refreshToken, AppUser user)
    {
        throw new NotImplementedException();
    }

    public bool ValidateRefreshToken(string refreshToken, AppUser user)
    {
        throw new NotImplementedException();
    }
}
