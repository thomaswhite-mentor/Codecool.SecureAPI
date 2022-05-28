using Codecool.SecureAPI.Model;
using Codecool.SecureAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Codecool.SecureAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("login")]
        public ActionResult<string> Login([FromForm] AuthLoginRequest authLoginRequest)
        {
            //Step 1:  validate the username
            var user = ValidateUserCredentials(authLoginRequest.Name, authLoginRequest.Password);
            if(user == null)
            {
                return Unauthorized();
            }
            // Step 2 : Create Token
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // the claims that
            var claimsforToken = new List<Claim>();
            claimsforToken.Add(new Claim("sub", user.Id.ToString()));
            claimsforToken.Add(new Claim("given_name", user.Name));
            claimsforToken.Add(new Claim(ClaimTypes.Role, user.Role));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsforToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
                ); 
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Ok(tokenToReturn);
        }
        private UserViewModel ValidateUserCredentials(string? name, string? password)
        {
            return new UserViewModel
            {
                Id = 1,
                Name = "Tamas",
                Role = "Admin"
            };          
        }
    }
}
