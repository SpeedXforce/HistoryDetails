using HistoryDetails.Repositories;
using HistoryDetails.Repositories.Commands;
using HistoryDetails.Repositories.Queries;
using HistoryDetails.Services.Commands;
using HistoryDetails.Services.Queries;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// Register Repository & Service
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();

app.MapControllers();

app.Run();
