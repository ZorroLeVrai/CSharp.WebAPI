using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APISecurity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _config;

    //record used to hold the username and password
    public record AuthenticationData(string? UserName, string? Password);
    public record UserData(int UserId, string UserName, string Title, string EmployeeId);

    public AuthenticationController(IConfiguration _config)
    {
        this._config = _config;
    }

    //This is going to pass in the username and password in the body (basic authentication)
    //api/Authentication/token
    [HttpPost("token")]
    [AllowAnonymous]
    public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
    {
        var user = ValidateCredentials(data);

        if (user is null)
        {
            return Unauthorized();
        }

        //the user is authorized => generate a token
        var token = GenerateToken(user);

        //sucessful action with the Token itself
        return Ok(token);
    }

    private string GenerateToken(UserData user)
    {
        //Get the value of "Authentication:SecretKey" and encode using SymmetricSecurityKey
        //: allows you to go inside a JSON object in the configuration
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey")));

        //now we are going to sign our credentials
        //the signature won't match if 1 bit of the token has been tamperered
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        //claims are data points about the user that were verified
        List<Claim> claims = new();
        //standard claim 'Subject'. A way to identify a user
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()));
        //standard claim 'UniqueName'
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.UserName));

        //add custom claims in the token
        claims.Add(new("Title", user.Title));
        claims.Add(new("employeeId", user.EmployeeId));

        //generate the token
        var token = new JwtSecurityToken(
            //who issued the token
            _config.GetValue<string>("Authentication:Issuer"),
            //who the audience is
            _config.GetValue<string>("Authentication:Audience"),
            //what claims has been added to this token
            claims,
            //when this token becomes valid
            DateTime.UtcNow,
            //when the token will expire
            DateTime.UtcNow.AddMinutes(1),
            signingCredentials);

        //returns a string which is going to be the actual token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private UserData? ValidateCredentials(AuthenticationData data)
    {
        //THIS IS NOT FOR PRODUCTION CODE - THIS IS ONLY FOR A DEMO
        //This is simulating authentication

        if (CompareValues (data.UserName, "King") &&
            CompareValues (data.Password, "Test123"))
        {
            //! specifies to the compiler that data.UserName cannot be null
            return new UserData(1, data.UserName!, "Business Owner", "E001");
        }

        if (CompareValues(data.UserName, "Queen") &&
            CompareValues(data.Password, "Test123"))
        {
            //! specifies to the compiler that data.UserName cannot be null
            return new UserData(1, data.UserName!, "Head of Security", "E002");
        }

        return null;
    }

    private bool CompareValues(string? actual, string expected)
    {
        if (actual is not null)
        {
            return actual.Equals(expected, StringComparison.InvariantCulture);
        }

        return false;
    }
}
