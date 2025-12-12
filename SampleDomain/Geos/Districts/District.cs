using Sample.Common.Database;
using Sample.Common.Domain;
using Dapper.Contrib.Extensions;

namespace Sample.Domain.Geos.Districts
{
    [Table("[dbo].[District]")]
    [DatabaseType(DatabaseTypes.Default)]
    public class District : Entity
    {
        [Key] [ExplicitKey]
        public long DistrictID { get; set; }
        public string? DistrictCode { get; set; }
        public string? DistrictName { get; set; }
        public string? CityCode { get; set; }
        public short Type { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}