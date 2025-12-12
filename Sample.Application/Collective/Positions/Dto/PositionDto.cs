using Sample.Common.FilterList;
using Sample.Common.ModelDto;

namespace Sample.Application.Collective.Positions.Dto
{
    public class PositionDto : AuditingDto
    {
        public long PositionID { get; set; }
        public long ManagementUnitID { get; set; }
        [Keyword]
        public string? Code { get; set; }
        [Keyword]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; }
        public bool? IsSystem { get; set; }
        public byte? Type { get; set; }
        public long? ModuleID { get; set; }
    }
}