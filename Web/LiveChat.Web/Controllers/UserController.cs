namespace LiveChat.Web.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using LiveChat.Services.Data;
    using LiveChat.Services.Data.Models.Users;
    using LiveChat.Web.Settings;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IOptions<JwtSettings> jwtSettings;

        public UserController(IUserService userService, IOptions<JwtSettings> jwtSettings)
        {
            this.userService = userService;
            this.jwtSettings = jwtSettings;
        }

        [HttpPost("/api/[controller]/Register")]
        [IgnoreAntiforgeryToken]
        public async Task<IdentityResult> RegisterUser(RegisterUserInputModel userInput)
        {
            var result = await this.userService.Register(userInput);
            return result;
        }

        [Authorize]
        [HttpDelete("/api/[controller]/Delete")]
        [IgnoreAntiforgeryToken]
        public async Task<IdentityResult> DeleteUser(string username)
        {
            var result = await this.userService.Delete(username);
            return result;
        }

        [Authorize]
        [HttpPut("/api/[controller]/UpdateUser")]
        [IgnoreAntiforgeryToken]
        public async Task<IdentityResult> UpdateUser(UpdateUserInputModel user, string username)
        {
            var result = await this.userService.Update(user, username);
            return result;
        }

        [HttpGet("/api/[controller]/Login")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await this.userService.Login(username, password);
            if (user == null)
            {
               return this.BadRequest(new { message = "Username or password is incorrect" });
            }

            // Authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(this.jwtSettings.Value.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                 new Claim(ClaimTypes.Name, user.UserName.ToString()),

                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 }),

                Expires = DateTime.UtcNow.AddDays(7),

                SigningCredentials = new SigningCredentials(
                 new SymmetricSecurityKey(key),
                 SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenAsString = tokenHandler.WriteToken(token);

            return this.Ok(tokenAsString);

            // should save it
        }

        [HttpGet("/api/[controller]/Logut")]
        [IgnoreAntiforgeryToken]
        public async Task Logout()
        {
            await this.userService.Logout();
        }
    }
}
