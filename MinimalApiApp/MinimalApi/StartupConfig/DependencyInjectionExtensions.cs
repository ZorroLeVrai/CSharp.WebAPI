using Microsoft.OpenApi.Models;

namespace MinimalApi.StartupConfig;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Sets the configuration of Swagger to be able to use Authentication
    /// </summary>
    /// <param name="builder"></param>
    public static void AddSwaggerServices(this WebApplicationBuilder builder)
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
}
