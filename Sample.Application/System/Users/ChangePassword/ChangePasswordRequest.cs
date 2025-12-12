using System.ComponentModel.DataAnnotations;

namespace Sample.Application.System.Users.ChangePassword
{
    public class ChangePasswordRequest
    {
        [Required]
        public string? CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string? ConfirmPassword { get; set; }
    }

}