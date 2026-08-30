using Microsoft.EntityFrameworkCore;
using SmartBank.Infrastructure.Data;
using SmartBank.Application.Auth.Interfaces;
using SmartBank.Infrastructure.Services;
using SmartBank.Application.Transactions.Interfaces;
using SmartBank.Infrastructure.Repositories;
using SmartBank.Application.Auth.Services;
using SmartBank.Application.Common.Authentication;
using SmartBank.Application.Accounts.Interfaces;
using SmartBank.Application.Accounts.Services;
using SmartBank.Application.Validators;
using SmartBank.Application.Transfers.Interfaces;
using SmartBank.Application.Transfers.Services;
using SmartBank.Application.Transactions.Services;
using SmartBank.Application.Notifications.Interfaces;
using SmartBank.Application.Notifications.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

    var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException(
        "Jwt ayarları bulunamadı.");

        builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();

builder.Services.AddScoped<
    ITransactionRepository,
    TransactionRepository>();

    builder.Services.AddScoped<
    ITransactionService,
    TransactionService>();

builder.Services.AddValidatorsFromAssemblyContaining<
    CreateAccountRequestValidator>();

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
