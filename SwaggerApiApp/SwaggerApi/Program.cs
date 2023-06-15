using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Our User API (this is our title)",
        Description = "This is the description about our API.",
        TermsOfService = new Uri("https://terms.conditions.com"),
        Contact = new OpenApiContact
        {
            Name = "Tim Corey (Contact Info)",
            Url = new Uri("https://www.info.com")
        },
        License = new OpenApiLicense
        {
            Name = "Cool License",
            Url = new Uri("https://www.info.com")
        }
    });
    //runs only once when the application loads
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //Custom themes
    //https://github.com/ostranme/swagger-ui-themes

    app.UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        opts.RoutePrefix = string.Empty;
        //set the stylesheet for Swagger
        opts.InjectStylesheet("/css/theme-monokai.css");
    });
}

app.UseHttpsRedirection();
//allows to use static files
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
