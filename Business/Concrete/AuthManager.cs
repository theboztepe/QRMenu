using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs.QR;
using Entities.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;

            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
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
                Status = true,
                QRCode = Guid.NewGuid().ToString().Replace("-","")
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

        public IDataResult<string> GetQRMenuCode()
        {
            User userInfo = _userService.GetQRMenuCode(Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value));
            return userInfo == null
                ? new ErrorDataResult<string>(Messages.UserNotFound)
                : new SuccessDataResult<string>(userInfo.QRCode, "");
        }

        public IDataResult<int?> GetQRCodeForUserId(QRCodeDto qrCode)
        {
            User userInfo = _userService.GetQRCodeForUserId(qrCode);
            return userInfo == null 
                ? new ErrorDataResult<int?>(Messages.QRNotFound)
                : new SuccessDataResult<int?>(userInfo.Id);
        }
    }
}