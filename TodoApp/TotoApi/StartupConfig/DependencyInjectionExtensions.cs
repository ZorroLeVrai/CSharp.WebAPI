using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TodoLibrary.DataAccess;

namespace TotoApi.StartupConfig
{
    public static class DependencyInjectionExtensions
    {
        public static void AddStandardServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.AddSwaggerServices();
        }

        /// <summary>
        /// Sets the configuration of Swagger to be able to use Authentication
        /// </summary>
        /// <param name="builder"></param>
        private static void AddSwaggerServices(this WebApplicationBuilder builder)
        {
            //security scheme
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Description = "JWT Authorization header info using bearer tokens",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    new string[] {}
                }
            };

            builder.Services.AddSwaggerGen(opts =>
            {
                opts.AddSecurityDefinition("bearerAuth", securityScheme);
                opts.AddSecurityRequirement(securityRequirement);
            });
        }

        public static void AddCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
            builder.Services.AddSingleton<ITodoData, TodoData>();
        }

        public static void AddAuthenticationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(opts =>
            {
                opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                        ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")))
                    };
                });
        }

        public static void AddHealthCheckServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddSqlServer(builder.Configuration.GetConnectionString("Default"));
        }
    }
}
