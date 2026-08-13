using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InventoryApi.Data;
using InventoryApi.Modules.Auth.Dtos;
using InventoryApi.Modules.Auth.Entities;
using Microsoft.IdentityModel.Tokens;

namespace InventoryApi.Modules.Auth.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public AuthResponseDto? Register(RegisterDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email)) return null;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return new AuthResponseDto { Message = "Usuario registrado exitosamente." };
        }

        public AuthResponseDto? Login(LoginDto dto)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == dto.Email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            string token = GenerateJwtToken(user);

            return new AuthResponseDto { Token = token, Message = "Login exitoso." };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(2), 
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}