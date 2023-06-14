using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using APISecurity.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(opts =>
{
    //add custom claim policy
    opts.AddPolicy(PolicyConstants.MustHaveEmployeeId, policy =>
    {
        //You must have this claim in the record in order to match the policy
        policy.RequireClaim("employeeId");
    });
    opts.AddPolicy(PolicyConstants.MustBeTheOwner, policy =>
    {
        //must have the claim title equals to "Business Owner"
        policy.RequireClaim("title", "Business Owner");
    });
    opts.AddPolicy(PolicyConstants.MustBeAVeteranEmployee, policy =>
    {
        //must have the claim title equals to "Business Owner"
        policy.RequireClaim("employeeId", "E001", "E002", "E003");
    });
    opts.FallbackPolicy = new AuthorizationPolicyBuilder()
        //if there is no policy applied, we at least need to require the user to be authenticated
        //so no need for the [Authorize] decorator
        .RequireAuthenticatedUser()
        .Build();

});
//Add a Service - services are dependency injections
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(opts =>
    {
        //we want to verify that what you gives us back is exactly what we gave you
        opts.TokenValidationParameters = new()
        {
            //We validate the issuer, the audience and issuerSigningKey
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//you have to check authentication before checking authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
