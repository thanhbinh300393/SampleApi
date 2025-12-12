using Sample.Common.Extentions;
using Dapper;
using System;
using System.Data;

namespace Sample.Common.Processing.DapperMappingHandler
{
    public class NullableIntHandler : SqlMapper.TypeHandler<int?>
    {
        public override void SetValue(IDbDataParameter parameter, int? value)
        {
            if (value.HasValue)
                parameter.Value = value.Value;
            else
                parameter.Value = DBNull.Value;
        }

        public override int? Parse(object value)
        {
            if (value == null || value is DBNull) return null;
            return value.ToIntOrNull();
        }
    }
}
