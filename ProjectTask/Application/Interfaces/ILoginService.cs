using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Interfaces
{
    public interface ILoginService
    {
        Task<(string Token, string? UserId, string Message)> UserLogin(UserLoginRequest request);
    }
}
