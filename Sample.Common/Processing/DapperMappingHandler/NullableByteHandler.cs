using Sample.Common.Extentions;
using Dapper;
using System;
using System.Data;

namespace Sample.Common.Processing.DapperMappingHandler
{
    public class NullableByteHandler : SqlMapper.TypeHandler<Byte?>
    {
        public override void SetValue(IDbDataParameter parameter, Byte? value)
        {
            if (value.HasValue)
                parameter.Value = value.Value;
            else
                parameter.Value = DBNull.Value;
        }

        public override Byte? Parse(object value)
        {
            if (value == null || value is DBNull) return null;
            return value.ToByteOrNull();
        }
    }
}
