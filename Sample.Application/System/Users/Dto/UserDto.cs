using Sample.Common.FilterList;
using Sample.Common.ModelDto;

namespace Sample.Application.System.Users.Dto
{
    public class UserDto : AuditingDto
    {
        public string? Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string? ConcurrencyStamp { get; set; }
        [Keyword]
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? PasswordHash { get; set; }
        [Keyword]
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [Keyword]
        public string? UserName { get; set; }
        [Keyword]
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public byte Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int Type { get; set; }
        public bool IsSystem { get; set; }
        public byte? PasswordResetStatus { get; set; }
        public string? Password { get; set; }
    }
}
