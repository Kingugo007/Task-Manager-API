using Serilog;
using TaskManager.API.Extensions;
using TaskManager.API.Middleware;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Services;
using TaskManager.Core.Utilities;
using HttpClientHandler = TaskManager.Infrastructure.HttpClientHandler;

try
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var isDevelopment = environment == Environments.Development;

    IConfiguration config = ConfigurationSetup.GetConfig(isDevelopment);
    LogSettings.SetUpSerilog(config);
    Log.Logger.Information("Application starting up");
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddSingleton(Log.Logger);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpClient<IHttpCommandHandlers, HttpClientHandler>();
    builder.Services.AddScoped<IHttpCommandHandlers, HttpClientHandler>();
    builder.Services.AddAutoMapper(typeof(MapperInitializer));
    builder.Services.AddScoped<ITaskServices, TaskServices>();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    // SeriLog: catch setup errors
    Log.Logger.Fatal(e.Message, "The application failed to start correctly");
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    Log.CloseAndFlush();
}