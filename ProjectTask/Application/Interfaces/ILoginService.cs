using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Interfaces
{
    public interface ILoginService
    {
        Task<(string Token, int? UserId, string Message)> UserLogin(UserLoginRequest request);
    }
}
