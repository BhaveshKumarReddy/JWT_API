using JWT_API.Data;
using JWT_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace JWT_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserDataProvider _userDataProvider = new UserDataProvider();
        public AuthorizationController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public User get(string name, string pass)
        {
            User login = new User()
            {
                Username = name,
                Password = pass
            };
            User user = _userDataProvider.GetUserDetail(login);
            return user;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                // add , Role = user.IsEmployee if u have role
                response = Ok(new LoginResponse { token = tokenString, User_Id = login.Username});
            }

            return response;
        }

        private string GenerateJSONWebToken(User userInfo)
        {

            if (userInfo is null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }
            List<Claim> claims = new List<Claim>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            claims.Add(new Claim("Username", userInfo.Username));
            /* In case u have role property in model
            if (userInfo.IsEmployee)
            {
                claims.Add(new Claim("role", "admin"));
            }
            else
            {
                claims.Add(new Claim("role", "POC"));

            }
            */
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(User login)
        {
            User user = _userDataProvider.GetUserDetail(login);
            return user;
        }
    }
}
