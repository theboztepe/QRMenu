using API.TokenConfig;
using Core.Entities.Concrete;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, SigningConfigurations signingConfigurations, TokenOptions tokenOptions);
    }
}
