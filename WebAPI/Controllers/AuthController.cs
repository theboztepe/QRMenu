using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using Entities.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenOptions tokenOptions)
        {
            IDataResult<User> userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            IDataResult<AccessToken> result = _authService.CreateAccessToken(userToLogin.Data, signingConfigurations, tokenOptions);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenOptions tokenOptions)
        {
            IResult userExists = _authService.UserExists(userForRegisterDto.Email, userForRegisterDto.Username);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            IDataResult<User> registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            IDataResult<AccessToken> result = _authService.CreateAccessToken(registerResult.Data, signingConfigurations, tokenOptions);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}