using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rca.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody] CredentialsModal credentialsModal)
        {
            if (credentialsModal.UserName == "admin" && credentialsModal.Password == "password")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@test.com"),
                    new Claim("Department", "HR"),
                    new Claim("admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmployementDate", "2023-01-01")
                };

                var expiryAt = DateTime.UtcNow.AddMinutes(10);

                return Ok(new
                {
                    access_token = CreateToken(claims, expiryAt),
                    expires_at = expiryAt
                });
            }

            return Unauthorized();
        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
        {
            var key = _configuration.GetValue<string>("Keys:SecretKey");
            var secretKey = Encoding.ASCII.GetBytes(key);

            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials
                (new SymmetricSecurityKey(secretKey), 
                SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
