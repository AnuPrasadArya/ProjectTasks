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
    public class RegisterService : IRegisterService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public RegisterService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
        public async Task<(string Token, string? UserId, string Message)> NewUserRegistration(UserRegistrationRequest request)
        {
            
            var IsExistingUser = await _db.Users.FirstOrDefaultAsync(r => r.Username == request.Username);
            if (IsExistingUser != null)
            {               
                return ("", "", "Username already exists");
            }
            string JwtToken = Helper.GenerateJwtToken(_config);
            using var hmac = new HMACSHA256();
            var user = new Users
            {
                Username = request.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password??string.Empty)),
                PasswordSalt = hmac.Key
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return (JwtToken ,request.Username, "Success");
        }
    }
}
