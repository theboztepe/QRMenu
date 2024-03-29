﻿using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using Entities.DTOs.QR;
using Entities.DTOs.User;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(string email, string username);
        IDataResult<AccessToken> CreateAccessToken(User user, SigningConfigurations signingConfigurations, TokenOptions tokenOptions);
        IDataResult<string> GetQRMenuCode();
        IDataResult<int?> GetQRCodeForUserId(QRCodeDto qrCode);
    }
}