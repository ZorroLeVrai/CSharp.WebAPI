using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//generates the Swagger set up
//this is only used to add information into the Swagger page
builder.Services.AddSwaggerGen(opts =>
{
    var title = "Our versioned API";
    var description = "This is a Web API that demonstrates versioning.";
    var terms = new Uri("https://localhost:7204/terms");
    var license = new OpenApiLicense()
    {
        Name = "This is my full license information or a link to it."
    };
    var contact = new OpenApiContact()
    {
        Name = "John Doe HelpDesk",
        Email = "contact@johndoe.com",
        Url = new Uri("https://www.johndoe.com")
    };

    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = $"{title} v1 (deprecated)",
        Description = description,
        TermsOfService = terms,
        License = license,
        Contact = contact
    });

    opts.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = $"{title} v2",
        Description = description,
        TermsOfService = terms,
        License = license,
        Contact = contact
    });
});
//add the API versioning service
builder.Services.AddApiVersioning(opts =>
{
    //when you don't specify a version, we would assume it is the default version
    opts.AssumeDefaultVersionWhenUnspecified = true;
    //specify the default version
    opts.DefaultApiVersion = new(2, 0);
    //to report the version in the response header
    opts.ReportApiVersions = true;
});

//configure our API explorer
builder.Services.AddVersionedApiExplorer(opts =>
{
    //specifies how we name our version v{Major}{Minor}{Patched}
    opts.GroupNameFormat = "'v'VVV";
    //for swagger allows to handle versions using a drop down
    opts.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        //list of available versions for Swagger
        opts.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
