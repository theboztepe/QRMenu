using API.TokenConfig;
using Core.Entities.Concrete;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        private DateTime _dtCreation;
        private DateTime _dtExpiration;
        public JwtHelper()
        {
        }

        public AccessToken CreateToken(User user, SigningConfigurations signingConfigurations, TokenOptions tokenOptions)
        {
            DateTime NowDate = DateTime.Now;

            _dtCreation = NowDate;
            _dtExpiration = _dtCreation +
                TimeSpan.FromSeconds(tokenOptions.AccessTokenExpiration);

            SecurityToken jwt = CreateJwtSecurityToken(tokenOptions, user, signingConfigurations, NowDate);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            string token = jwtSecurityTokenHandler.WriteToken(jwt);

            AccessToken accessToken = new()
            {
                Token = token,
                Expiration = _dtExpiration
            };

            return accessToken;
        }

        private SecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningConfigurations signingCredentials, DateTime NowDate)
        {

            JwtSecurityTokenHandler handler = new();
            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenOptions.Issuer,
                Audience = tokenOptions.Audience,
                SigningCredentials = signingCredentials.SigningCredentials,
                Subject = SetClaims(user, NowDate),
                NotBefore = _dtCreation,
                Expires = _dtExpiration
            });

            return securityToken;
        }

        private static ClaimsIdentity SetClaims(User user, DateTime NowDate)
        {
            ClaimsIdentity identity = new(
                 new GenericIdentity(user.Username, "Login"),
                 new[] {
                           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                           new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                           new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                           new Claim(JwtRegisteredClaimNames.AuthTime, NowDate.ToString("yyyy-MM-dd HH:mm:ss")),
                 }
              );

            return identity;
        }
    }
}