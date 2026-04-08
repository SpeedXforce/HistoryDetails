using HistoryDetails.Repositories;
using HistoryDetails.Repositories.Commands;
using HistoryDetails.Repositories.Queries;
using HistoryDetails.Services.Commands;
using HistoryDetails.Services.Queries;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;


var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithProcessId()
    .WriteTo.Console(
    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}]" + "{Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
    path: "logs/history-app-.log",
    rollingInterval: RollingInterval.Day,
    retainedFileCountLimit: 7,
    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}]" + "{Message:lj}{NewLine}{Exception}"
    ).
    WriteTo.ApplicationInsights(
        connectionString: configuration
            ["ApplicationInsights:ConnectionString"],
        telemetryConverter: TelemetryConverter.Traces
    ).
    CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration
        ["ApplicationInsights:ConnectionString"];
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://20.235.205.244")
              .AllowAnyHeader()
              .AllowAnyMethod();

    });
});
builder.Services.AddDbContext<HistoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IHistoryCommandRepository, HistoryCommandRepository>();
builder.Services.AddScoped<IHistoryCommandService, HistoryCommandService>();

builder.Services.AddScoped<IHistoryQueryRepository, HistoryQueryRepository>();
builder.Services.AddScoped<IHistoryQueryService, HistoryQueryService>();

// Add services to the container.

builder.Services.AddControllers();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting HistoryDetails API");
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}

