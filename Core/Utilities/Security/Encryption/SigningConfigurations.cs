using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace API.TokenConfig
{
    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (RSACryptoServiceProvider provider = new(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(
                    Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}