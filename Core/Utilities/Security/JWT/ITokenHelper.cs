using Core.Entities.Concrete;
using Core.Utilities.Security.Encryption;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, SigningConfigurations signingConfigurations, TokenOptions tokenOptions);
    }
}
