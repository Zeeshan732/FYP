using neuro_kinetic_backend.DTOs.Auth;

namespace neuro_kinetic_backend.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<bool> ValidateTokenAsync(string token);
    }
}



