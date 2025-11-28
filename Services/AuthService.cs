using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProyectoTecWeb.Models;
using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Repositories;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoTecWeb.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository users, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _users = users;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(bool ok, LoginResponseDto? response)> LoginAsync(LoginDto dto)
        {
            var user = await _users.GetByEmailAddress(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("⚠️ Intento de login fallido: Email no existe ({Email})", dto.Email);
                return (false, null);
            }

            var ok = BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash);
            if (!ok)
            {
                _logger.LogWarning("⚠️ Intento de login fallido: Contraseña incorrecta para ({Email})", dto.Email);
                return (false, null);
            }

            // Generar par access/refresh
            var (accessToken, expiresIn, jti) = GenerateJwtToken(user);
            var refreshToken = GenerateSecureRefreshToken();

            var refreshDays = int.Parse(_configuration["Jwt:RefreshDays"] ?? "14");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshDays);
            user.RefreshTokenRevokedAt = null;
            user.CurrentJwtId = jti;
            await _users.UpdateAsync(user);

            _logger.LogInformation("✅ Usuario logueado exitosamente: {Email} ({Id})", user.Email, user.Id);

            var resp = new LoginResponseDto
            {
                User = new UserDto { Id = user.Id, Username = user.Username, Email = user.Email },
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresIn,
                TokenType = "Bearer"
            };

            return (true, resp);
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existing = await _users.GetByEmailAddress(dto.Email);
            if (existing != null)
            {
                _logger.LogWarning("⚠️ Intento de registro duplicado: {Email}", dto.Email);
                throw new Exception("El email ya está registrado");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Username = dto.Username,
                Role = dto.Role
            };
            await _users.AddAsync(user);
            _logger.LogInformation("👤 Nuevo usuario registrado: {Email} con rol {Role}", dto.Email, dto.Role);
            return user.Id.ToString();
        }

        public async Task<(bool ok, LoginResponseDto? response)> RefreshAsync(RefreshRequestDto dto)
        {
            // Buscar usuario que tenga ese refresh token (simple)
            var user = await _users.GetByRefreshToken(dto.RefreshToken);
            if (user == null)
            {
                _logger.LogWarning("⚠️ Refresh fallido: Token no encontrado");
                return (false, null);
            }

            // Validaciones de refresh
            if (user.RefreshToken != dto.RefreshToken || user.RefreshTokenRevokedAt.HasValue || user.RefreshTokenExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("⚠️ Refresh fallido: Token inválido o expirado para usuario {Id}", user.Id);
                return (false, null);
            }
            // Rotación: generar nuevo access + refresh y revocar el anterior
            var (accessToken, expiresIn, jti) = GenerateJwtToken(user);
            var newRefresh = GenerateSecureRefreshToken();
            var refreshDays = int.Parse(_configuration["Jwt:RefreshDays"] ?? "14");

            user.RefreshToken = newRefresh;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshDays);
            user.RefreshTokenRevokedAt = null; // seguimos activo
            user.CurrentJwtId = jti;
            await _users.UpdateAsync(user);

            _logger.LogInformation("🔄 Token refrescado exitosamente para usuario {Id}", user.Id);
            var resp = new LoginResponseDto
            {
                User = new UserDto { Id = user.Id, Username = user.Username, Email = user.Email },
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = newRefresh,
                ExpiresIn = expiresIn,
                TokenType = "Bearer"
            };

            return (true, resp);
        }

        private (string token, int expiresInSeconds, string jti) GenerateJwtToken(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection["Key"]!;
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var expireMinutes = int.Parse(jwtSection["ExpiresMinutes"] ?? "60");

            var jti = Guid.NewGuid().ToString();

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
            };

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(expireMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return (jwt, (int)TimeSpan.FromMinutes(expireMinutes).TotalSeconds, jti);
        }

        private static string GenerateSecureRefreshToken()
        {
            // 64 bytes aleatorios en Base64Url
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Base64UrlEncoder.Encode(bytes);
        }
    }
}
