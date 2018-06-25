using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using dnd_buddy_backend.Controllers;

namespace dnd_buddy_backend.Models
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly DataContext _context;
        private AuthSettings _authSettings;

        public AuthController(DataContext context, IOptions<AuthSettings> authSettings)
        {
            _context = context;
            _authSettings = authSettings.Value;
        }

        private string CreateToken(User user)
        {
            //string secret = _authSettings.SECRET; //DEV
            string secret = Environment.GetEnvironmentVariable("SECRET"); //PROD
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] { new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()) };
            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private OkObjectResult AuthResult(User user)
        {
            string token = CreateToken(user);

            return Ok(
                new
                {
                    Username = user.Username,
                    Token = token
                });
        }

        [Authorize]
        [HttpGet("authUser")]
        public IActionResult Get()
        {
            string id = HttpContext.User.Claims.First().Value;
            User _user = _context.User.SingleOrDefault(u => u.UserId.ToString() == id);

            _user.Password = null;

            return Ok(_user);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUser user)
        {
            string username = user.Username;
            string password = user.Password;

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Unauthorized();
            }

            User _user = _context.User.SingleOrDefault(x => x.Username == username);

            if (_user == null) return Unauthorized();

            var passwordHasher = new PasswordHasher<User>();
            if(!String.Equals(password, _user.Password))
            {
                return Unauthorized();
            }

            return AuthResult(_user);
        }

        public class LoginUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
