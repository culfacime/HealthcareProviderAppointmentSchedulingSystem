using DotNetEnv;
using Healthcare.Core.DTOs;
using Microsoft.IdentityModel.Tokens;
using Healthcare.Core.Entities;
using Healthcare.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Healthcare.API.Extensions;

namespace Healthcare.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/V{version:apiversion}/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IGenericService<User> _userService;

        public TokenController(IGenericService<User> userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// User Token Creator
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UserTokenCreate")]
        public async Task<IActionResult> UserTokenCreate(string username, string password)
        {
            var messageList = new List<string>();
            var user = await _userService.FirstOrDefaultAsync(k => k.UserName == username);

            if (user == null)
            {
                messageList.Add("User not found");
                return CreateActionResult(CustomResponseDto<List<User>>.Fail(401, messageList));
            }

            if (user != null && user.Password != password) // intentionally not encrypted for now
            {
                messageList.Add("Not authorized");
                return CreateActionResult(CustomResponseDto<List<User>>.Fail(401, messageList));
            }
            if (user != null && !user.Active)
            {
                messageList.Add("User is not active");
                return CreateActionResult(CustomResponseDto<List<User>>.Fail(401, messageList));
            }


            var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, Env.GetString("JWT_Subject")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserId.ToString())
                    };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_Key")));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
               Env.GetString("JWT_Issuer"),
              Env.GetString("JWT_Audience"),
                claims,
                expires: DateTime.UtcNow.AddDays(5),
                signingCredentials: signIn);
            var tokenModel = new TokenDto { Token = new JwtSecurityTokenHandler().WriteToken(token) };
            messageList.Add("Login is successfull.");
            return CreateActionResult(CustomResponseDto<TokenDto>.Success(200, tokenModel, messageList));
        }

        private IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            if (response.StatusCode == 204)
            {
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };
            }
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

    }
}
