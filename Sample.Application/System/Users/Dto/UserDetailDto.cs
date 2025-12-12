using Sample.Common.ModelDto;

namespace Sample.Application.System.Users.Dto
{
    public class UserDetailDto : AuditingDto
    {
        public virtual string? Id { get; set; }
        public virtual string? PhoneNumber { get; set; }
        public virtual string? Email { get; set; }
        public virtual string? UserName { get; set; }

        public string? Address { get; set; }
        public string? FullName { get; set; }
        public bool IsAdminAccount { get; set; }
        public Guid? UserGroupId { get; set; }
        public bool IsActive { get; set; }

        public string? Avatar { get; set; }

    }
}
