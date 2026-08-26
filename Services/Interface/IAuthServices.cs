using System.Security.Claims;

public interface IAuthServices
{
    Task<AuthResponse> RegisterAsync(RegisterDto dto);
    Task<AuthResponse> LoginAsync(LoginDto dto);
    Task<ServicesResponse> LogoutAsync(string userId);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenDto dto);
    Task<ServicesResponse> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<ServicesResponse> ResetPasswordAsync(ResetPasswordDto dto);
    Task<ServicesResponse> ConfirmEmailAsync(string userId, string token);
    Task<ServicesResponse> ChangePasswordAsync(string userId, ChangePasswordDto dto);
    Task<IEnumerable<GetProfile>> GetAllUsersAsync();
    Task<GetProfile> GetProfileAsync(string userId);
    Task<ServicesResponse> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    Task<ServicesResponse> AssignRoleAsync(string userId, string roleName);
    Task<ServicesResponse> RemoveRoleAsync(string userId, string roleName);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}