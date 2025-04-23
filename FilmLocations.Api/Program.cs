using System.Data;
using Films.Api.Data;
using Films.Api.Managers.Contracts;
using Films.Api.Managers.Implementations;
using Films.Api.Repositories.Contracts;
using Films.Api.Repositories.Implementations;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new() 
    { 
        Title = "Films API",
        Version = "v1" 
    }));
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddAutoMapper(typeof(Program)); 
builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Configure SQLite in-memory database
var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();
builder.Services.AddSingleton<IDbConnection>(connection);

// Register repositories
builder.Services.AddScoped<IFilmRepository, FilmRepository>();

// Register managers
builder.Services.AddScoped<IFilmManager, FilmManager>();

var app = builder.Build();

// Seed the database at startup
DatabaseSeeder.SeedDatabase("Data/Film_Locations_in_San_Francisco.csv", connection);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWebClient");
app.MapControllers();
app.Run();