using ProjectTask.Application.DTOs;

namespace ProjectTask.Application.Interfaces
{
    public interface IRegisterService
    {
        Task<(string Token, string? UserId, string Message)> NewUserRegistration(UserRegistrationRequest request);
        Task<string> NewUserRegistrationJson(string jsonRequest);

    }
}
