using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase {
        private readonly IConfiguration _configuration;

        public class AuthenticationRequestBody {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        public class CityInfoUser {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string City { get; set; }

            public CityInfoUser(int userId, string username, string firstname, string lastname, string city) {
                UserId = userId;
                Username = username;
                Firstname = firstname;
                Lastname = lastname;
                City = city;
            }
        }

        public AuthenticationController(IConfiguration configuration) {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody) {
            // 1. validate user
            var user = ValidateUserCredentials(authenticationRequestBody.Username, authenticationRequestBody.Password);

            if (user == null) {
                return Unauthorized();
            }

            // 2. create token
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.Firstname));
            claimsForToken.Add(new Claim("family_name", user.Lastname));
            claimsForToken.Add(new Claim("city", user.City));

            var jwtToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
            );

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(tokenToReturn);
        }

        private CityInfoUser ValidateUserCredentials(string? username, string? password) {
            return new CityInfoUser(
                1,
                username ?? "",
                "Serge",
                "Ratke",
                "Mannheim"
            );
        }
    }
}
