using System;
using System.Collections.Generic;

namespace Sample.Common.UserSessions
{
    public class UserInfo
    {
        public UserInfo()
        {
        }

        public string UserId { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Type { get; set; }

        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public long UserGroupId { get; set; }
        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsTrial { get; set; }
        public double? ExpirationDay => ((ExpirationDate ?? new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59)).Date - DateTime.Now.Date).Days;

        public string? ManagementUnitCode { get; set; }
        public long ManagementUnitID { get; set; }
        public string? ManagementUnitName { get; set; }

        public bool IsManagementUnit { get; set; }
    }
}
