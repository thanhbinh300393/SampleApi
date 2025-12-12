using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Domain.System.Users.Dtos
{
    public class UserInput
    {      
        public string? Id { get; set; }
        public int AccessFailedCount {  get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public byte Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Type { get; set; }
        public bool IsSystem { get; set; }
        public string? EmailAppsheet { get; set; }
        public string? Password { get; set; }
    }
}
