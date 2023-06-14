using TotoApi.StartupConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddStandardServices();
builder.AddAuthenticationServices();
builder.AddHealthCheckServices();
builder.AddCustomServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//allow access to health without authentication
app.MapHealthChecks("/health")
    .AllowAnonymous();

app.Run();
