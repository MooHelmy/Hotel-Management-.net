using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class TokenService(IConfiguration configuration) : ITokenService
{

    public string CreateToken(ApplicationUser user, IList<string> roles)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("fullName", user.FullName),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var signingKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(jwtSettings["SigningKey"]!));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expiryMinutes = double.Parse(jwtSettings["ExpiryMinutes"]!);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds

        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        //using

        // كلمة using هنا معناها إن الكائن rng هيتنضف من الذاكرة تلقائيًا أول ما الميثود تخلص
        //   شغلها(لأنه بيستخدم موارد نظام تشغيل حساسة زي ملفات أو اتصالات، فمهم تقفلها صح بعد الاستخدام).
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}