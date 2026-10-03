using Backend_CSharp.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Npgsql;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrWhiteSpace(databaseUrl))
{
    throw new InvalidOperationException(
        "No se encontró DATABASE_URL."
    );
}

var uri = new Uri(databaseUrl);

var userInfo = uri.UserInfo.Split(':', 2);

var connectionStringBuilder = new NpgsqlConnectionStringBuilder
{
    Host = uri.Host,
    Port = 5432,
    Database = uri.AbsolutePath.Trim('/'),
    Username = Uri.UnescapeDataString(userInfo[0]),
    Password = userInfo.Length > 1
        ? Uri.UnescapeDataString(userInfo[1])
        : "",
    SslMode = SslMode.Require
};

var connectionString = connectionStringBuilder.ConnectionString;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5025";

builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => new
{
    mensaje = "Microservicio C# funcionando correctamente"
});

app.MapControllers();

app.Run();