using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using MinimalApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MinimalApi.Endpoints;

public static class AuthenticationEndpoints
{
    public static void AddAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/api/token", (IConfiguration config, [FromBody] AuthenticationData data) =>
        {
            var user = ValidateCredentials(data);

            if (user is null)
                return Results.Unauthorized();

            string token = GenerateToken(user, config);

            return Results.Ok(token);
        });
    }


    private static string GenerateToken(UserData user, IConfiguration config)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.GetValue<string>("Authentication:SecretKey")));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.UserName));
        claims.Add(new(JwtRegisteredClaimNames.GivenName, user.FirstName));
        claims.Add(new(JwtRegisteredClaimNames.FamilyName, user.LastName));

        var token = new JwtSecurityToken(
            config.GetValue<string>("Authentication:Issuer"),
            config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(1),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UserData? ValidateCredentials(AuthenticationData data)
    {
        //THIS IS NOT PRODUCTION CODE - REPLACE THIS WITH A CALL TO YOU AUTH SYSTEM
        if (CompareValues(data.UserName, "King") &&
            CompareValues(data.Password, "Test123"))
        {
            return new UserData(1, "King", "Doe", data.UserName!);
        }

        if (CompareValues(data.UserName, "Queen") &&
            CompareValues(data.Password, "Test123"))
        {
            return new UserData(2, "Queen", "Doe", data.UserName!);
        }

        return null;
    }

    private static bool CompareValues(string? actual, string expected)
    {
        return actual is not null && actual.Equals(expected);
    }
}
