using Sample.Common.Extentions;
using Dapper;
using System;
using System.Data;

namespace Sample.Common.Processing.DapperMappingHandler
{
    public class NullableLongHandler : SqlMapper.TypeHandler<long?>
    {
        public override void SetValue(IDbDataParameter parameter, long? value)
        {
            if (value.HasValue)
                parameter.Value = value.Value;
            else
                parameter.Value = DBNull.Value;
        }

        public override long? Parse(object value)
        {
            if (value == null || value is DBNull) return null;
            return value.ToLongOrNull();
        }
    }
}
