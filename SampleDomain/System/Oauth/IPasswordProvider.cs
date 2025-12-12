using Sample.Common.Dependency;
namespace Sample.Domain.System.Oauth
{
    public interface IPasswordProvider : ITransientDependency
    {
        string HashPassword(string password, string securityStamp = "");
        string OldHashPassword(string password, string securityStamp = "");
        bool Verify(string hash, string password, string securityStamp = "");
        bool OldVerify(string hash, string password, string securityStamp = "");
    }
}
