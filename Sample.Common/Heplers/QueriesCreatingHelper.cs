using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Sample.Common.Heplers
{
    public static class QueriesCreatingHelper
    {
        public static string GetValue(object? valueObj)
        {
            if (valueObj == null)
            {
                return "NULL";
            }
            if (valueObj is bool)
            {
                return (bool)valueObj == true ? "1" : "0";
            }
            else if (valueObj is Guid)
            {
                return $"'{((Guid)valueObj).ToString().ToLower().PreventSqlInjection()}'";

            }
            else if (valueObj is Guid?)
            {
                return $"'{((Guid)valueObj).ToString().ToLower().PreventSqlInjection()}'";

            }
            else if (valueObj is DateTime)
            {
                return $"'{((DateTime)valueObj).ToString("yyyy-MM-dd HH:mm:ss")}'";
            }
            else if (valueObj is double)
            {
                return $"'{((double)valueObj).ToString("F")}'";
            }
            else if (valueObj is float)
            {
                return $"'{((float)valueObj).ToString("F")}'";
            }
            else if (valueObj is decimal)
            {
                return $"'{((decimal)valueObj).ToString("F")}'";
            }
            else if (valueObj is Enum)
            {
                return $"'{((Enum)valueObj).GetHashCode()}'";
            }
            else if (valueObj is int)
            {
                return $"'{valueObj}'";
            }
            else
            {
                //Encoding utf8 = new UTF8Encoding(true);
                return $"N'{valueObj.ToString().PreventSqlInjection()}'";
            }
        }

        public static string? PreventSqlInjection(this string? value)
        {
            return value?.Replace("'", "''");
        }


        public static string GetTableName<T>()
        {
            var tableName = string.Empty;
            if (typeof(T).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TableAttribute attributeTable)
            {
                tableName = attributeTable.Name;
            }
            else if (typeof(T).GetCustomAttributes(typeof(Dapper.Contrib.Extensions.TableAttribute), true).FirstOrDefault() is Dapper.Contrib.Extensions.TableAttribute attributeTableContrib)
            {
                tableName = attributeTableContrib.Name;
            }
            else
            {
                tableName = typeof(T).Name;
            }
            return tableName;
        }
    }
}
