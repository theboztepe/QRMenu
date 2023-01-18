using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs.User;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            HashingHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new()
            {
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            User userToCheck = _userService.GetByUsername(userForLoginDto.Username);
            return userToCheck == null
                ? new ErrorDataResult<User>(Messages.UserNotFound)
                : !userToCheck.Status
                ? new ErrorDataResult<User>(Messages.UserClosed)
                : !HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt)
                ? new ErrorDataResult<User>(Messages.UserNotFound)
                : new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }

        public IResult UserExists(string email, string username)
        {
            return _userService.GetByMail(email) != null || _userService.GetByUsername(username) != null
                ? new ErrorResult(Messages.UserAlreadyExists)
                : new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user, SigningConfigurations signingConfigurations, TokenOptions tokenOptions)
        {
            AccessToken accessToken = _tokenHelper.CreateToken(user, signingConfigurations, tokenOptions);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

    }
}