using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using MediatR;
using Recchx.Users.Infrastructure.Persistence;
using Recchx.Users.Application.Services;
using Recchx.Users.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Recchx.WebAPI.Infrastructure.Persistence;
using Recchx.WebAPI.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Recchx API",
        Version = "v1",
        Description = "AI-powered recruitment outreach platform API"
    });
    
    // Add JWT authentication to Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add DbContext with PostgreSQL
builder.Services.AddDbContext<RecchxDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

builder.Services.AddAuthorization();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Recchx.Users.Application.Commands.RegisterUser.RegisterUserCommand).Assembly));

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Recchx.Users.Application.Commands.RegisterUser.RegisterUserCommandValidator).Assembly);

// Add AutoMapper (for future use)
builder.Services.AddAutoMapper(typeof(Program));

// Register Repositories
builder.Services.AddScoped<IUserRepository, Recchx.WebAPI.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, Recchx.WebAPI.Infrastructure.Repositories.UserProfileRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, Recchx.WebAPI.Infrastructure.Repositories.RefreshTokenRepository>();
builder.Services.AddScoped<IUserSessionRepository, Recchx.WebAPI.Infrastructure.Repositories.UserSessionRepository>();

// Register Services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IDeviceFingerprintService, DeviceFingerprintService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<RecchxDbContext>("database");

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseTokenValidation();
app.UseAuthorization();

app.MapControllers();

// Map Health Check endpoint
app.MapHealthChecks("/health");

// Add a simple test endpoint
app.MapGet("/api/info", () => new
{
    Name = "Recchx API",
    Version = "1.0.0",
    Environment = app.Environment.EnvironmentName,
    Timestamp = DateTime.UtcNow
})
.WithName("GetApiInfo")
.WithOpenApi();

try
{
    Log.Information("Starting Recchx API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
