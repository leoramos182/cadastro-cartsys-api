using System.Security.Claims;
using System.Text;
using CadastroCartsys.Api;
using CadastroCartsys.Crosscutting.Config;
using CadastroCartsys.Data;
using CadastroCartsys.Data.Repositories;
using CadastroCartsys.Domain.Users;
using CadastroCartsys.Domain.Users.Commands.Handlers;
using CadastroCartsys.Domain.Users.Validators;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});
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

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngularClient", builder => {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAngularClient");
    app.UseRouting();
    app.UseAuthorization();
    app.UseAuthentication();
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

app.MapGet("/user", (ClaimsPrincipal user) =>
    {
        return Results.Ok($"Bem-Vindo {user.Identity.Name}!");
    })
    .RequireAuthorization();

app.Run();