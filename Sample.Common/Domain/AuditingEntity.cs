using Sample.Common.Heplers;
using System;

namespace Sample.Common.Domain
{
    public interface IAuditingEntity
    {
        [MapIfNull]
        DateTime? CreatedDate { get; set; }
        [MapIfNull]
        string? CreatedBy { get; set; }
        [MapIfNull]
        DateTime? ModifiedDate { get; set; }
        [MapIfNull]
        string? ModifiedBy { get; set; }
    }

    public class AuditingEntity : Entity, IAuditingEntity
    {
        public AuditingEntity()
        {
            CreatedDate = DateTime.Now;
        }

        [MapIfNull]
        public DateTime? CreatedDate { get; set; }
        [MapIfNull]
        public string? CreatedBy { get; set; }
        [MapIfNull]
        public DateTime? ModifiedDate { get; set; }
        [MapIfNull]
        public string? ModifiedBy { get; set; }
    }
}
