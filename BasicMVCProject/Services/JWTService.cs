using BasicMVCProject.Interfaces;
using DAL.Entities.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BasicMVCProject.Services
{
    public class JWTService(IConfiguration configuration) : IJWTService
    {
        public string GenerateToken(UserEntity user)
        {
            var jwtToken = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim("Email", user.Email),
                    new Claim("Login", user.Login),
                },
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSecurityKey"])), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
