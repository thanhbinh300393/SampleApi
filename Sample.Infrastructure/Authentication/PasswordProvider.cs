using Sample.Common.Security;
using Sample.Domain.System.Oauth;

namespace Sample.Infrastructure.Authentication
{
    public class PasswordProvider : IPasswordProvider
    {
        public PasswordProvider()
        {
        }

        public string HashPassword(string password, string securityStamp = "")
        {
            return CryptographyHelper.GetHash(securityStamp + password, CryptographyAlgorithms.SHA256);
        }

        public string OldHashPassword(string password, string securityStamp = "")
        {
            return CryptographyHelper.GetHash(securityStamp + password, CryptographyAlgorithms.MD5);
        }

        public bool Verify(string hash, string password, string securityStamp = "")
        {
            return hash?.ToLower() == HashPassword(password, securityStamp)?.ToLower();
        }

        public bool OldVerify(string hash, string password, string securityStamp = "")
        {
            return hash?.ToLower() == OldHashPassword(password, securityStamp)?.ToLower();
        }
    }
}
