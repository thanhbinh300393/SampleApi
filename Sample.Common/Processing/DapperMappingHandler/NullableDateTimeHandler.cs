using Sample.Common.Extentions;
using Dapper;
using System;
using System.Data;

namespace Sample.Common.Processing.DapperMappingHandler
{
    public class NullableDateTimeHandler : SqlMapper.TypeHandler<DateTime?>
    {
        public override void SetValue(IDbDataParameter parameter, DateTime? value)
        {
            if (value.HasValue)
                parameter.Value = value.Value;
            else
                parameter.Value = DBNull.Value;
        }

        public override DateTime? Parse(object value)
        {
            if (value == null || value is DBNull) return null;
            return value.ToDateTimeOrNull();
        }
    }
}
