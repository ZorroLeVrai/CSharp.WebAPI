using MonitoringApi.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using WatchDog;

//https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
//https://github.com/IzyPro/WatchDog

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddCheck<RandomHealthCheck>("Site Health Check")
    .AddCheck<RandomHealthCheck>("Database Health Check");
//add watch dog services
builder.Services.AddWatchDogServices();

builder.Services.AddHealthChecksUI(opts =>
{
    //the UI is going to point to /health (Name + the URL we want to monitor)
    opts.AddHealthCheckEndpoint("api", "/health");
    //evaluate the health check every 5 seconds
    opts.SetEvaluationTimeInSeconds(5);
    //if you get 2 failures in 10 seconds you don't need to report that the app is still down
    opts.SetMinimumSecondsBetweenFailureNotifications(10);

    //we are going to store information about the health check in memory
}).AddInMemoryStorage();

var app = builder.Build();

//logs any unhandeled exception
app.UseWatchDogExceptionLogger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//we give a specific URL for the health check
app.MapHealthChecks("/health", new HealthCheckOptions {
    //here is how to format what data you are getting back
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

//configure Watchdog
app.UseWatchDog(opts =>
{
    opts.WatchPageUsername = app.Configuration.GetValue<string>("WatchDog:UserName");
    opts.WatchPagePassword = app.Configuration.GetValue<string>("WatchDog:Password");
    opts.Blacklist = "health";
});

app.Run();
