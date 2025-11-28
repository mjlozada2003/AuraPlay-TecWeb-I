using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.Repositories; 
using ProyectoTecWeb.Services;     
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Variables de entorno (si quieres usar .env para JWT o PORT)
Env.Load();

// 2. Configuración de puerto (Railway/Docker/local)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// 3. Controladores y endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 4. Swagger con JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Music API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// 5. CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// 6. Base de datos: usar SIEMPRE la connection string del appsettings
// OJO: la clave debe coincidir con "ConnectionStrings": { "Default": "..." }
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Host=localhost;Port=5432;Database=musicdb;Username=musicuser;Password=supersecretpassword123";

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(connectionString));

// 7. JWT (puedes mezclar appsettings o variables de entorno)
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? Environment.GetEnvironmentVariable("JWT_KEY")
    ?? "EstaEsUnaClaveSuperSecretaYLoSuficientementeLargaParaHmacSha256!";
// Configuración DB
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(connectionString))
{
    // Lógica para Railway (DATABASE_URL)
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':', 2);
    var user = Uri.UnescapeDataString(userInfo[0]);
    var pass = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
    var builderCs = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = user,
        Password = pass,
        Database = uri.AbsolutePath.Trim('/'),
        SslMode = SslMode.Disable // Cambiar a Require si Railway lo exige en producción
    };
    connectionString = builderCs.ConnectionString;
}
else
{
    // Lógica Local (Docker variables)
    var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "musicdb";
    var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "musicuser";
    var dbPass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "supersecret";
    var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";

    connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPass}";
}

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
    });

// 8. Políticas
builder.Services.AddAuthorization(opt =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
});

// 9. Registro de repositorios y servicios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// --- INYECCIÓN DE DEPENDENCIAS ---
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();       
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

var app = builder.Build();

// 11. Pipeline HTTP

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();