using Sample.Common.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Domain.System.Users
{
    [Table("AspNetUsers")]
    public class DapperUser : AuditingEntity
    {
        [Key]
        [Dapper.Contrib.Extensions.ExplicitKey]
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public long? UserGroupID { get; set; }
        public bool IsActive { get; set; }
        public int Type { get; set; }
        public bool IsSystem { get; set; }
        public int AccessFailedCount { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
