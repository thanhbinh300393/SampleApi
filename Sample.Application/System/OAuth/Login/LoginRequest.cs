using System.ComponentModel.DataAnnotations;

namespace Sample.Application.System.Users.OAuth.Login
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "FieldIsRequired")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "FieldIsRequired")]
        public string Password { get; set; } = string.Empty;
        public bool IsRemember { get; set; }
    }
}
