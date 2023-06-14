using AspNetCoreRateLimit;
using ApiProtection.StartupConfig;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//add the service for response caching - the client (browser) can cache the data as well
builder.Services.AddResponseCaching();

//Use the memory cache to cache our call information - this will work per server
builder.Services.AddMemoryCache();
builder.AddRateLimitServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//tells the system that we are going to use the response caching
app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.UseIpRateLimiting();

app.Run();
