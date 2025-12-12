using Sample.Common.Database;
using Sample.Common.Domain;
using Dapper.Contrib.Extensions;

namespace Sample.Domain.Geos.Wards
{
    [Table("[dbo].[Ward]")]
    [DatabaseType(DatabaseTypes.Srp)]
    public class Ward : Entity
    {
        [Key] [ExplicitKey]
        public long WardID { get; set; }
        public string? WardCode { get; set; }
        public string? WardName { get; set; }
        public string? DistrictCode { get; set; }
        public string? CityCode { get; set; }
        public short Type { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}