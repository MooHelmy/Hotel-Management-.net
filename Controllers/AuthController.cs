using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/auth")]
[Authorize]
public class AuthController(IAuthServices authServices) : ControllerBase
{

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        var result = await authServices.RegisterAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var result = await authServices.LoginAsync(dto);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
        var result = await authServices.LogoutAsync(userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> GetAllUsers()
    {
        var users = await authServices.GetAllUsersAsync();
        return Ok(users);
    }
    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken(RefreshTokenDto dto)
    {
        var result = await authServices.RefreshTokenAsync(dto);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        var result = await authServices.ForgotPasswordAsync(dto);
        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var result = await authServices.ResetPasswordAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("confirm-email")]
    public async Task<ActionResult> ConfirmEmail(string userId, string token)
    {
        var result = await authServices.ConfirmEmailAsync(userId, token);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
        var result = await authServices.ChangePasswordAsync(userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
        var profile = await authServices.GetProfileAsync(userId);
        return Ok(profile);
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult> UpdateProfile(UpdateProfileDto dto)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
        var result = await authServices.UpdateProfileAsync(userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    // بضيف الدور للمستخدم المسجل بالفعل
    public async Task<ActionResult> AssignRole([FromQuery] string userId, [FromQuery] string roleName)
    {
        var result = await authServices.AssignRoleAsync(userId, roleName);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("remove-role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RemoveRole([FromQuery] string userId, [FromQuery] string roleName)
    {
        var result = await authServices.RemoveRoleAsync(userId, roleName);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}