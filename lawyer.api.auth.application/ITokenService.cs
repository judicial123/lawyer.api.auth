namespace lawyer.api.auth.application;

public interface ITokenService
{
    string GenerateToken(AuthUserDto user);
}

public class AuthUserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
}