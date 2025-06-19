using lawyer.api.auth.application;
using lawyer.api.auth.datastore.mssql;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest dto)
    {
        // Buscar usuario por correo
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        // Verificar contraseña
        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            throw new Exception("Credenciales inválidas");

        // Mapear a AuthUserDto
        var userDto = new AuthUserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };

        // Generar token
        var token = _tokenService.GenerateToken(userDto);

        return new AuthResponse { Token = token };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"Error al registrar usuario: {errors}");
        }

        var userDto = new AuthUserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };

        var token = _tokenService.GenerateToken(userDto);

        return new AuthResponse { Token = token };
    }
}