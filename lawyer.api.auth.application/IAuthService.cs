namespace lawyer.api.auth.application;


// Interfaces
public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest dto);
    Task<AuthResponse> RegisterAsync(RegisterRequest dto);
}