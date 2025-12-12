using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Application.System.OAuth.FindUserByEmails;
using Sample.Application.System.OAuth.ForgotPassword;
using Sample.Application.System.OAuth.LoginOauths;
using Sample.Application.System.OAuth.ResetPassword;
using Sample.Application.System.OAuth.SendEmailOtps;
using Sample.Application.System.OAuth.SetUserSession;
using Sample.Application.System.OAuth.SignUpUsers;
using Sample.Application.System.OAuth.UpdatePasswordForgots;
using Sample.Application.System.OAuth.VerifyOtps;
using Sample.Application.System.Users.Dto;
using Sample.Application.System.Users.OAuth.Login;
using Sample.Application.System.Users.OAuth.Logout;
using Sample.Application.System.Users.OAuth.RefreshToken;
using Sample.Common.Web;
using Sample.Domain.System.Users.Dtos;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Sample.Api.Controllers.System.OAuth
{
    [Route("api/oauth")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;
        private readonly Common.UserSessions.IUserSession _userSession;

        public OAuthController(
            IMediator mediator,
            ILogger logger,
            Common.UserSessions.IUserSession userSession)
        {
            _mediator = mediator;
            _logger = logger;
            _userSession = userSession;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new LoginQuery(request));
            return Ok(result);
        }

        [HttpPost("login-apple-oauth")]
        public async Task<IActionResult> LoginAppleAuth(string idToken)
        {
            var result = await _mediator.Send(new LoginAppleQuery(idToken));
            return Ok(result);
        }

        [HttpPost("login-google-oauth")]
        public async Task<IActionResult> LoginGoogleOauth(string idToken)
        {
            var result = await _mediator.Send(new LoginGoogleQuery(idToken));
            return Ok(result);
        }

        [HttpPost("login-facebook-oauth")]
        public async Task<IActionResult> LoginFacebookOauth(string idToken)
        {
            var result = await _mediator.Send(new LoginFacebookQuery(idToken));
            return Ok(result);
        }

        [AppAuthorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _mediator.Send(new RefreshTokenQuery());
            return Ok(result);
        }

        [AppAuthorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _mediator.Send(new LogoutCommand());
            return Ok(result);
        }

        [HttpPut("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserInput request)
        {
            var result = await _mediator.Send(new ForgotPasswordCommand(request));
            return Ok(result);
        }

        [AppAuthorize]
        [HttpPost("set-session/{moduleId}")]
        public async Task<IActionResult> SetSession(long moduleId)
        {
            var result = await _mediator.Send(new SetUserSessionCommand(moduleId));
            return Ok(result);
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateSignUp([FromBody] SignUpUserRequest request)
        {
            var result = await _mediator.Send(new SignUpUserCommand(request));
            if (result == null)
                return NoContent();
            return Created("", result);
        }

        [HttpGet("find-email")]
        [AllowAnonymous]
        public async Task<IActionResult> FindEmail(string email)
        {
            var result = await _mediator.Send(new FindUserByEmailQuery(email));
            if (result == null)
                return NoContent();
            return Ok(result);
        }

        [HttpPost("send-email-forgot-password")]
        public async Task<IActionResult> SendEmail(string email, string userId)
        {
            await _mediator.Send(new SendEmailOtpCommand(email, userId));
            return Ok();
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(string otp, string userId)
        {
            var result = await _mediator.Send(new VerifyOtpCommand(otp, userId));
            return Ok(result);
        }

        [HttpPost("update-password-forgot")]
        public async Task<IActionResult> UpdatePasswordForgot(string password, string userId)
        {
            var result = await _mediator.Send(new UpdatePasswordForgotCommand(userId, password));
            return Ok(result);
        }

        [HttpPost("reset-password/{userID}/{password}")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string password)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(userId, password));
            return Ok(result);
        }
    }
}