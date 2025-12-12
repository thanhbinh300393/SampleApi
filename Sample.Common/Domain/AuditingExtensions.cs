using Sample.Common.UserSessions;
using System;

namespace Sample.Common.Domain
{
    public static class AuditingExtensions
    {
        public static void SetAuditCreation(this IAuditingEntity auditingEntity, IUserSession useSession)
        {
            auditingEntity.CreatedDate = DateTime.Now;
            auditingEntity.CreatedBy = useSession.UserInfo?.UserId;
        }

        public static void SetAuditUpdation(this IAuditingEntity auditingEntity, IUserSession useSession)
        {
            auditingEntity.ModifiedDate = DateTime.Now;
            auditingEntity.ModifiedBy = useSession.UserInfo?.UserId;
        }
    }
}
