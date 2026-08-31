using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ikea.Data;
using ikea.Models;
using ikea.DTO;

namespace ikea.Sources
{
    public class AuthSource
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthSource(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.PasswordHash != password)
                return null;

            return CreateToken(user);
        }

        public async Task<string?> RegisterAsync(User newUser)
        {
            if (await _context.Users.AnyAsync(u => u.Email == newUser.Email))
                return null;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var cart = new Cart { UserId = newUser.Id };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreateToken(newUser);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "Customer"),
                new Claim("FirstName", user.FirstName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}