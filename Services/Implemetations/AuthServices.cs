using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AuthServices(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService,
    IConfiguration configuration
    ) : IAuthServices
{
    public async Task<AuthResponse> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return new AuthResponse { Success = false, Message = "يوجد حساب بهذا الإيميل بالفعل" };
        }
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            StreetName = dto.StreetName,
            City = dto.City,
            CreatedAt = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return new AuthResponse
            {
                Success = false,
                Message = string.Join(" | ", result.Errors.Select(e => e.Description))
            };
        }
        await userManager.AddToRoleAsync(user, "User");
        return new AuthResponse { Success = true, Message = "تم إنشاء الحساب بنجاح" };
    }
    public async Task<AuthResponse> LoginAsync(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return new AuthResponse { Success = false, Message = "البيانات غير صحيحة" };
        }


        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
        if (result.IsLockedOut)
        {
            return new AuthResponse { Success = false, Message = "الحساب مقفول مؤقتًا بسبب محاولات دخول فاشلة كثيرة" };
        }

        if (!result.Succeeded)
        {
            return new AuthResponse { Success = false, Message = "البيانات غير صحيحة" };
        }

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.CreateToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return new AuthResponse
        {
            Success = true,
            Message = "تم تسجيل الدخول بنجاح",
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:ExpiryMinutes"]!))
        };
    }
    public async Task<ServicesResponse> LogoutAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ServicesResponse(false, "User not found");
        }
        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        await userManager.UpdateAsync(user);
        return new ServicesResponse(true, "تم تسجيل الخروج بنجاح");
    }
    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var principal = GetPrincipalFromExpiredToken(dto.AccessToken);
        var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var user = await userManager.FindByIdAsync(userId!);
        if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            return new AuthResponse { Success = false, Message = "Refresh Token غير صالح أو منتهي" };
        }


        var roles = await userManager.GetRolesAsync(user);
        var newAccessToken = tokenService.CreateToken(user, roles);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return new AuthResponse
        {
            Success = true,
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:ExpiryMinutes"]!))
        };


    }
    public async Task<ServicesResponse> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        const string genericMessage = "لو الإيميل ده مسجل عندنا، هيوصلك رابط استرجاع الباسورد";

        // ملحوظة أمان: بنرجع نفس الرسالة سواء الإيميل مسجل دخول أو غير مسجل دخول
        if (user == null)
        {
            return new ServicesResponse(true, genericMessage);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"https://yourapp.com/reset-password?email={dto.Email}&token={Uri.EscapeDataString(token)}";
        // await emailService.SendAsync(dto.Email, "Password Reset", resetLink);

        return new ServicesResponse(true, genericMessage);
        // await emailService.SendAsync(dto.Email, "Password Reset", resetLink);
    }
    public async Task<ServicesResponse> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return new ServicesResponse(false, "طلب غير صالح");
        }

        var result = await userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
        if (!result.Succeeded)
        {
            return new ServicesResponse(false, string.Join(" | ", result.Errors.Select(e => e.Description)));
        }

        return new ServicesResponse(true, "تم تغيير كلمة المرور بنجاح");
    }
    public async Task<ServicesResponse> ConfirmEmailAsync(string userId, string token)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ServicesResponse(false, "مستخدم غير موجود");
        }

        var result = await userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            return new ServicesResponse(false, "فشل تأكيد البريد الإلكتروني، الرابط غير صالح أو منتهي");
        }

        return new ServicesResponse(true, "تم تأكيد البريد الإلكتروني بنجاح");

    }
    public async Task<ServicesResponse> ChangePasswordAsync(string userId, ChangePasswordDto dto)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ServicesResponse(false, "User not found");
        }

        var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            return new ServicesResponse(false, string.Join(" | ", result.Errors.Select(e => e.Description)));
        }

        return new ServicesResponse(true, "تم تغيير كلمة المرور بنجاح");
    }
    public async Task<GetProfile> GetProfileAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new ItemNotFoundException($"User with id {userId} was not found");
        }
        return new GetProfile
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            StreetName = user.StreetName,
            City = user.City,
            EmailConfirmed = user.EmailConfirmed
        };


    }
    public async Task<IEnumerable<GetProfile>> GetAllUsersAsync()
    {
        var users = userManager.Users.ToList();

        return users.Select(u => new GetProfile
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName,
            StreetName = u.StreetName,
            City = u.City,
            EmailConfirmed = u.EmailConfirmed
        });
    }
    public async Task<ServicesResponse> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ServicesResponse(false, "User not found");
        }
        if (dto.FullName != null) user.FullName = dto.FullName;
        if (dto.StreetName != null) user.StreetName = dto.StreetName;
        if (dto.City != null) user.City = dto.City;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return new ServicesResponse(false, string.Join(" | ", result.Errors.Select(e => e.Description)));
        }

        return new ServicesResponse(true, "تم تحديث البيانات بنجاح");
    }
    public async Task<ServicesResponse> AssignRoleAsync(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ServicesResponse(false, "مستخدم غير موجود");
        }

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
        var result = await userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            return new ServicesResponse(false, string.Join(" | ", result.Errors.Select(e => e.Description)));
        }

        return new ServicesResponse(true, $"تم إعطاء الدور {roleName} للمستخدم بنجاح");
    }
    public async Task<ServicesResponse> RemoveRoleAsync(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ServicesResponse(false, "مستخدم غير موجود");
        }
        await userManager.RemoveFromRoleAsync(user, roleName);
        return new ServicesResponse(true, $"تم إزالة الدور {roleName}");
    }
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SigningKey"]!)),
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = false // الفرق هنا: مش بنتحقق من انتهاء الصلاحية
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("توكن غير صالح");
        }

        return principal;
    }


}