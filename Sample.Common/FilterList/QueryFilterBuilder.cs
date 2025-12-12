using Dapper;
using Dapper.Contrib.Extensions;
using Sample.Common.Heplers;
using System.Data;
using System.Reflection;

namespace Sample.Common.FilterList
{
    public static class QueryFilterBuilder
    {
        public static string Where<T>(IList<FilterValue> filters, string? keyWord = null)
            where T : class
        {
            var props = typeof(T).GetProperties();
            var conditionAnds = new List<string>();
            var conditionAndsStr = "";
            var filterValids = filters.Where(x => props.Any(t => t.Name?.ToLower() == x.PropertyName?.ToLower())).ToList();
            foreach (var item in filterValids)
            {
                var conditionItem = new List<string>();
                string fieldName = $"[{item.PropertyName}]";
                var prop = props.FirstOrDefault(x => item.PropertyName.ToLower() == x.Name.ToLower());
                if (prop?.PropertyType == typeof(DateTime) || prop?.PropertyType == typeof(DateTime?))
                {
                    fieldName = $"cast (datediff (day, 0, [{item.PropertyName}]) as datetime)";
                }
                if (prop?.PropertyType == typeof(string) && !item.HasTransform)
                {
                    fieldName = $"UPPER([{item.PropertyName}])";
                }
                if (item.Values != null)
                {
                    foreach (var value in item.Values)
                    {
                        if (prop?.PropertyType == typeof(string) && !item.HasTransform)
                        {
                            conditionItem.Add($"{fieldName} {FilterOperationConverter.GetConditionSql(item.Operator, value?.ToString()?.ToUpperInvariant())}");
                        }
                        else
                        {
                            conditionItem.Add($"{fieldName} {FilterOperationConverter.GetConditionSql(item.Operator, value)}");
                        }
                    }
                }
                if ((item.Values?.Count ?? 0) <= 0)
                {
                    conditionItem.Add($"{fieldName} {FilterOperationConverter.GetConditionSql(item.Operator, null)}");
                }

                conditionAnds.Add($"({string.Join(item.HasAnd ? " AND " : " OR ", conditionItem)})");
            }
            if (conditionAnds.Count > 0)
                conditionAndsStr = $"AND {string.Join(" AND ", conditionAnds)}";

            var whereKeyworkClause = "";
            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                var keywordProps = props.Where(x => x.CustomAttributes.Any(T => T.AttributeType.FullName == typeof(KeywordAttribute).FullName)).ToList();

                whereKeyworkClause = string.Join(" OR ", keywordProps.Select(x => $"UPPER([{x.Name}]) like N'%{keyWord.PreventSqlInjection()?.Trim().ToUpper()}%'").ToArray());
                if (!string.IsNullOrWhiteSpace(whereKeyworkClause))
                    whereKeyworkClause = $" AND ({whereKeyworkClause})";
            }

            if (!string.IsNullOrWhiteSpace(conditionAndsStr) || !string.IsNullOrWhiteSpace(whereKeyworkClause))
                return $"1 = 1 {conditionAndsStr} {whereKeyworkClause}";
            return $"1 = 1 ";
        }

        private static bool IsHaveAttribute(this PropertyInfo prop, Type attributeType)
        {
            return prop.CustomAttributes.Any(T => T.AttributeType.FullName == attributeType.FullName);
        }

        private static string Order<T>(IList<OrderValue> orderValues) where T : class
        {
            var props = typeof(T).GetProperties();
            var orders = orderValues
                .Where(x => props.Any(t => t.Name.ToLower() == x.PropertyName.ToLower()))
                .OrderBy(x => x.Index)
                .Select(x => $"[{x.PropertyName}] {(x.Type == OrderTypes.asc ? "ASC" : "DESC")}")
                .ToList();

            var propToOrder = props.Where(x =>
                    !x.IsHaveAttribute(typeof(System.ComponentModel.DataAnnotations.KeyAttribute))
                    && !x.IsHaveAttribute(typeof(ComputedAttribute))
                    && !x.Name.ToLower().EndsWith("id")).FirstOrDefault();
            var orderDefault = propToOrder?.Name ?? props.FirstOrDefault()?.Name;
            if (!string.IsNullOrEmpty(orderDefault) && !orders.Any(x => x.ToLower().Contains(orderDefault.ToLower())))
                orders.Add($"[{orderDefault}]");
            return $"{string.Join(",", orders)}";
        }

        private static string Filter<T>(FilterRequest filterOption, string viewName, string? customWhere = null) where T : class
        {
            var tableName = GetSqlCoreOrTable<T>(viewName);
            var noLockClause = string.IsNullOrWhiteSpace(viewName) ? " WITH (NOLOCK) " : "";
            var paginClause = filterOption.IsFull ? "" : $"OFFSET {(filterOption.Page - 1) * filterOption.Limit} ROWS FETCH NEXT  {filterOption.Limit} ROWS ONLY";
            var orderClause = $"ORDER BY {Order<T>(filterOption.Orders)}";
            string sql;
            if (string.IsNullOrWhiteSpace(customWhere))
                sql = $"SELECT * FROM {tableName} {noLockClause} WHERE {Where<T>(filterOption.Filters, filterOption.Keyword)} {orderClause} {paginClause} ;";
            else
                sql = $"SELECT * FROM {tableName} {noLockClause} WHERE {customWhere} {orderClause} {paginClause} ;";
            return sql;
        }

        private static string Count<T>(IList<FilterValue> filters, string? keyWord = null, string? viewName = null, string? customWhere = null) where T : class
        {
            var tableName = GetSqlCoreOrTable<T>(viewName);
            string sql;
            if (string.IsNullOrEmpty(customWhere))
                sql = $"SELECT COUNT(*) FROM {tableName} WHERE {Where<T>(filters, keyWord)} ;";
            else sql = $"SELECT COUNT(*) FROM {tableName} WHERE {customWhere} ;";
            return sql;
        }

        private static string? Summary<T>(IList<FilterValue> filters, string? keyWord, IList<string> summaryFields, string viewName) where T : class
        {
            var props = typeof(T).GetProperties();
            var tableName = GetSqlCoreOrTable<T>(viewName);
            var selectClauses = summaryFields
                .Where(x => props.Any(t => t.Name.ToLower() == x.ToLower()))
                .Select(x => $"SUM([{x}]) AS '{x}'").ToList();
            if (selectClauses.Count <= 0) return null;
            var sql = $"SELECT {string.Join(",", selectClauses)} FROM {tableName} WHERE {Where<T>(filters, keyWord)} ;";
            return sql;
        }

        private static string GetSqlCoreOrTable<T>(string? viewName, bool isAlias = true) where T : class
        {
            var tableName = viewName;
            if (string.IsNullOrWhiteSpace(viewName))
                tableName = QueriesCreatingHelper.GetTableName<T>();

            if (isAlias)
                return $"{tableName} f";
            else return tableName ?? "";
        }

        public async static Task<FilterResult<TDto>> RunAsync<TEntity, TDto>(this IDbConnection connection,
                    FilterRequest filterOption,
                    IDbTransaction? transaction = null,
                    string? viewName = null,
                    string? customWhereClause = null)
            where TEntity : class where TDto : class
        {
            viewName = GetSqlCoreOrTable<TEntity>(viewName, false);

            var sqlList = Filter<TDto>(filterOption, viewName, customWhereClause);
            var list = (await connection.QueryAsync<TDto>(sqlList, transaction: transaction)).ToList();

            int totalRecords = 0;
            if (filterOption.IsFull)
            {
                totalRecords = list.Count;
            }
            else
            {
                var sqlCount = Count<TDto>(
                    filterOption.Filters,
                    filterOption.Keyword,
                    viewName,
                    customWhereClause
                );
                totalRecords = await connection.QueryFirstAsync<int>(sqlCount, transaction: transaction);
            }

            object summary;
            if (filterOption.SummaryFields.Count > 0)
            {
                var sqlSummary = Summary<TEntity>(
                    filterOption.Filters,
                    filterOption.Keyword,
                    filterOption.SummaryFields,
                    viewName
                );
                summary = await connection.QueryFirstAsync<object>(sqlSummary ?? "", transaction: transaction);
            }
            else
            {
                summary = new { };
            }

            return new FilterResult<TDto>
            {
                IsFull = filterOption.IsFull,
                Limit = filterOption.Limit,
                Page = filterOption.Page,
                TotalRecords = totalRecords,
                List = list,
                Summary = summary
            };
        }

        public async static Task<List<TDto>> GetAllAsync<TEntity, TDto>(this IDbConnection connection,
              FilterRequest filterOption,
              IDbTransaction? transaction = null,
              string? viewName = null) where TEntity : class where TDto : class
        {
            filterOption.IsFull = true;
            viewName = GetSqlCoreOrTable<TEntity>(viewName, false);

            var sql = Filter<TDto>(filterOption, viewName);

            var result = await connection.QueryAsync<TDto>(sql, transaction: transaction);

            return result.ToList();
        }

        public async static Task<TDto?> GetAsync<TEntity, TDto>(this IDbConnection connection,
                    FilterRequest filterOption,
                    IDbTransaction? transaction = null,
                    string? viewName = null) where TEntity : class where TDto : class
        {
            viewName = GetSqlCoreOrTable<TEntity>(viewName, false);
            filterOption.Page = 1;
            filterOption.Limit = 1;
            var sql = Filter<TDto>(filterOption, viewName);
            var result = await connection.QueryAsync<TDto>(sql, transaction: transaction);
            return result.FirstOrDefault();
        }
    }
}
