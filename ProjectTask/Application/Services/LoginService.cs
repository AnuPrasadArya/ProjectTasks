using Microsoft.EntityFrameworkCore;
using ProjectTask.Application.DTOs;
using ProjectTask.Application.Interfaces;
using ProjectTask.Domain.Entities;
using ProjectTask.Helpers;
using ProjectTask.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

namespace ProjectTask.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public LoginService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
        public async Task<(string Token, string? UserId, string Message)> UserLogin(UserLoginRequest request)
        {

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return ("", "", "Invalid Username");
            }
            using var hmac = new HMACSHA256(user.PasswordSalt);

            var passwordBytes = Encoding.UTF8.GetBytes(request.Password ?? string.Empty);
            var computedHash = hmac.ComputeHash(passwordBytes);

            if (!computedHash.SequenceEqual(user.PasswordHash))
            {
                return ("", "", "Invalid Username or Password");
            }
            string jwtToken = Helper.GenerateToken(user, _config);

            return (jwtToken, user.Username, "Success");

        }
    }
}
