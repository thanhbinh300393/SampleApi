using System;

namespace Sample.Common.ModelDto
{
    public interface IAuditingDto
    {
        DateTime? CreatedDate { get; set; }
        string? CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        string? ModifiedBy { get; set; }
    }

    public class AuditingDto : IAuditingDto
    {
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
