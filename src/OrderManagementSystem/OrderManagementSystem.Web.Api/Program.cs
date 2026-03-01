//usingleri ekledik.
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderManagementSystem.Core.Interface;
using OrderManagementSystem.Infrastructure.Data;
using OrderManagementSystem.Infrastructure.Repositories;
using OrderManagementSystem.Infrastructure.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using OrderManagementSystem.Web.Api.Middleware;
using OrderManagementSystem.Web.Api.Hangfire;
using OrderManagementSystem.Web.Api.Redis;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Redis connection
builder.Services.AddRedisCache(builder.Configuration);

//RabbitMQ baðlantýsý
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // Docker compose içinden eriþimde host adý "rabbit"
        cfg.Host("rabbit", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

//serilog baðlantýsý
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//DbContext baðlantýsý ekledik.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//DL kayýtlarý ekledik.
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();


//Auth Config
var JwtSection = builder.Configuration.GetSection("Jwt");
var key = JwtSection["Key"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = JwtSection["Issuer"],
        ValidAudience = JwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});
builder.Services.AddAuthorization();

//hangfire
builder.Services.AddHangfireSetup(builder.Configuration);

var app = builder.Build();

//Middleware ekledik.
app.UseMiddleware<GlobalExceptionMiddleware>();

//Health Endpoint
app.MapGet("/health", () => Results.Ok(new { status="ok"}));

//request login
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
var enableSwagger = app.Configuration.GetValue<bool>("Swagger:Enabled");
if (app.Environment.IsDevelopment() || enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//hangfire
app.UseHangfireSetup(app.Environment);

app.MapControllers();

//seederý çalýþtýrma.
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

app.Run();
public partial class Program { }
