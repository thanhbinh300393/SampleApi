using Sample.Common.Database;
using Sample.Common.Domain;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
using TableAttribute = System.ComponentModel.DataAnnotations.Schema.TableAttribute;


namespace Sample.Domain.Collective.Positions
{
    [Table("Position")]
    [Sequence("[dbo].[Seq_Position_PositionID]")]
    [DatabaseType(DatabaseTypes.Default)]
    public partial class Position : AuditingEntity
    {
        [Key]
        [Dapper.Contrib.Extensions.ExplicitKey]
        public long PositionID { get; set; }
        public long? ManagementUnitID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public bool? IsSystem { get; set; }
        public byte? Type { get; set; }
        public long? ModuleID { get; set; }
    }
}
