using Sample.Common.Database;
using Sample.Common.Domain;
using Dapper.Contrib.Extensions;
using Serilog;

namespace Sample.Domain.Geos.Cities
{
    [Table("[dbo].[City]")]
    [DatabaseType(DatabaseTypes.Default)]
    public class City : Entity
    {
        [Key] [ExplicitKey]
        public long CityID { get; set; }
        public string? CityCode { get; set; }
        public string? CityName { get; set; }
        public string? ZipCode { get; set; }
        public short Type { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}