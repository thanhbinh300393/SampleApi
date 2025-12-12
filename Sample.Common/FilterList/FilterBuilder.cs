using Sample.Common.Heplers;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Sample.Common.FilterList
{
    public static class FilterBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) })!;
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) })!;
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) })!;

        public static IEnumerable<T> Where<T>(this IEnumerable<T> list, IList<FilterValue> filters)
        {
            var expression = GetExpression<T>(filters)?.Compile();

            if (expression != null)
                return list.Where(expression);
            return list;
        }

        public static Expression<Func<T, bool>>? GetExpression<T>(IList<FilterValue> filters)
        {
            var props = typeof(T).GetProperties();
            filters = filters.Where(x => props.Any(y => y.Name.ToLowerFirstChar() == x.PropertyName.ToLowerFirstChar())).ToList();
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression? exp = null;

            if (filters.Count == 1)
                exp = GetExpression<T>(param, filters[0]);
            else if (filters.Count == 2)
                exp = GetExpression<T>(param, filters[0], filters[1]);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                        exp = GetExpression<T>(param, filters[0], filters[1]);
                    else
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression GetExpression<T>(ParameterExpression param, FilterValue filter)
        {
            var member = Expression.Property(param, filter.PropertyName);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType); // 1

            if (!converter.CanConvertFrom(typeof(string))) // 2
                throw new NotSupportedException();

            var propertyValue = (filter.Values == null || filter.Values.FirstOrDefault() == null)
                ? null : converter.ConvertFromInvariantString(filter.Values.FirstOrDefault()?.ToString() ?? ""); // 3
            var constant = Expression.Constant(propertyValue);
            var valueExpression = Expression.Convert(constant, propertyType); // 4

            var expression = GetExpressionOperation(filter.Operator, member, valueExpression);

            if (filter.Values != null)
            {
                for (int i = 1; i < filter.Values.Count; i++)
                {
                    var propertyValueItem = filter.Values[i] == null ? null : converter.ConvertFromInvariantString(filter.Values[i]?.ToString() ?? ""); // 3
                    var constantItem = Expression.Constant(propertyValueItem);
                    var valueExpressionItem = Expression.Convert(constantItem, propertyType); // 4

                    if (filter.HasAnd)
                        expression = Expression.And(expression, GetExpressionOperation(filter.Operator, member, valueExpressionItem));
                    else expression = Expression.Or(expression, GetExpressionOperation(filter.Operator, member, valueExpressionItem));
                }
            }

            return expression;
        }


        private static Expression GetExpressionOperation(FilterOperations operation, MemberExpression member, UnaryExpression valueExpression)
        {
            switch (operation)
            {
                case FilterOperations.Equals:
                    return Expression.Equal(member, valueExpression);

                case FilterOperations.NotEquals:
                    return Expression.NotEqual(member, valueExpression);

                case FilterOperations.GreaterThan:
                    return Expression.GreaterThan(member, valueExpression);

                case FilterOperations.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, valueExpression);

                case FilterOperations.LessThan:
                    return Expression.LessThan(member, valueExpression);

                case FilterOperations.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, valueExpression);

                case FilterOperations.Contains:
                    return Expression.Call(member, containsMethod, valueExpression);

                case FilterOperations.StartsWith:
                    return Expression.Call(member, startsWithMethod, valueExpression);

                case FilterOperations.EndsWith:
                    return Expression.Call(member, endsWithMethod, valueExpression);
            }
            return null;
        }

        private static BinaryExpression GetExpression<T>
        (ParameterExpression param, FilterValue filter1, FilterValue filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }

        public static string GetParamUrl(FilterRequest filter)
        {
            var paramUrl = "";
            if (filter == null)
                paramUrl += "full";
            else
            {
                if (filter.IsFull)
                    paramUrl += "full";
                else
                {
                    paramUrl += $"&page={filter.Page}";
                    paramUrl += $"&limit={filter.Limit}";
                }
                if (filter.Keyword != null && filter.Keyword.Length > 0)
                {
                    paramUrl += $"&q={filter.Keyword}";
                }

                foreach (var item in filter.Filters)
                {
                    paramUrl += $"f.${ item.PropertyName}:${ FilterOperationConverter.GetAcroymOperater(item.Operator)}={String.Join('|', item.Values)}";
                }

                filter.Orders = filter.Orders.OrderBy(x => x.Index).ToList();

                var orderValues = filter.Orders.OrderBy(x => x.Index).Select(x => $"{ (x.Type == OrderTypes.desc ? "-" : "") }{ x.PropertyName}").ToList();
                if (orderValues != null && orderValues.Count > 0)
                    paramUrl += $"&sort={String.Join(',', orderValues.ToArray())}";
            }
            return paramUrl;
        }

    }
}
