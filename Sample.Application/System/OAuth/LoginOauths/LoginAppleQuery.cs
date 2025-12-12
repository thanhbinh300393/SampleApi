using Sample.Common.CQRS.Queries;
using Sample.Common.UserSessions;

namespace Sample.Application.System.OAuth.LoginOauths;

public class LoginAppleQuery : QueryBase<UserInfo>
{
    public string AppleToken { get; set; }

    public LoginAppleQuery(string appleToken)
    {
        AppleToken = appleToken;
    }
}

public class AppleLoginRequest
{
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }

    public string IdentityToken { get; set; } = null!;
    public string AuthorizationCode { get; set; } = null!;
}