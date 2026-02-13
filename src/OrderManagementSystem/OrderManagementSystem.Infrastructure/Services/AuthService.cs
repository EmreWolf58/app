using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.Core.Dtos;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Interface;
using OrderManagementSystem.Infrastructure.Data;


namespace OrderManagementSystem.Infrastructure.Services
{
    public class AuthService: IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<AppUser> _hasher = new();

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task <LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Username == request.Username);

            if (user == null) return null;

            var vertify = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if(vertify == PasswordVerificationResult.Failed) return null;

            var JwtSection = _config.GetSection("Jwt");
            var issuer = JwtSection["Issuer"]!;
            var audience = JwtSection["Audience"]!;
            var key = JwtSection["Key"]!;

            var expiresMinutes = int.Parse(JwtSection["ExpiresMinutes"] ?? "60");
            var expiresAtUtc = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: issuer,
              audience: audience,
              claims: claims,
              expires: expiresAtUtc,
              signingCredentials: creds
             );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse
            {
                Token = tokenStr,
                Username = user.Username,
                Role = user.Role,
                ExpiresAtUtc = expiresAtUtc
            };
        }
    }
}
