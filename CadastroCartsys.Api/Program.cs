using System.Security.Claims;
using CadastroCartsys.Api;
using CadastroCartsys.Crosscutting.Config;
using CadastroCartsys.Data;
using CadastroCartsys.Data.Repositories;
using CadastroCartsys.Domain.Contracts;
using CadastroCartsys.Domain.Users;
using CadastroCartsys.Domain.Users.Commands.Handlers;
using CadastroCartsys.Domain.Users.Validators;
using CadastroCartsys.Infra.Services;
using FluentValidation.AspNetCore;
using Jwks.Manager.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<UserCommandHandler>();
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var jwTokenConfig = new JwTokenConfig();
builder.Configuration.GetSection("JwTokenConfig").Bind(jwTokenConfig);
builder.Services.AddSingleton(jwTokenConfig);
builder.Services.AddScoped<IJwTokenService, JwTokenService>();

builder.Services.AddControllers().AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>());

builder.Services.AddControllers().AddFluentValidation(fv =>
    fv.RegisterValidatorsFromAssemblyContaining<UpdateUserCommandValidator>());


builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("Default")));

builder.Services
    .AddAuthentication()
    .AddBearerToken();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

app.UseHttpsRedirection();
app.AppUseMigrations();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/login", (string username) =>
{
    var claimsPrincipal = new ClaimsPrincipal(
        new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Name, username) },
            BearerTokenDefaults.AuthenticationScheme
        )
    );
    return Results.SignIn(claimsPrincipal);
});

app.MapGet("/user", (ClaimsPrincipal user) =>
    {
        return Results.Ok($"Bem-Vindo {user.Identity.Name}!");
    })
    .RequireAuthorization();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}