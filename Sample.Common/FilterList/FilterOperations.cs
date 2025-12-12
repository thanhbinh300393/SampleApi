using Sample.Common.Heplers;
using System;

namespace Sample.Common.FilterList
{
    public enum FilterOperations
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        NotContains,
        StartsWith,
        EndsWith
    }

    public static class FilterOperationConverter
    {
        public static FilterOperations Get(string stringValue, bool useAcroym = false)
        {
            if (!useAcroym)
            {
                object type;
                if (!Enum.TryParse(typeof(FilterOperations), stringValue, true, out type))
                    type = FilterOperations.Equals;
                return (FilterOperations)type;
            }
            else
            {
                if (stringValue.Contains(":"))
                    stringValue = stringValue.Substring(0, stringValue.IndexOf(":"));
                switch (stringValue.ToLower())
                {

                    case "neq":
                        return FilterOperations.NotEquals;
                    case "gt":
                        return FilterOperations.GreaterThan;
                    case "lt":
                        return FilterOperations.LessThan;
                    case "gte":
                        return FilterOperations.GreaterThanOrEqual;
                    case "lte":
                        return FilterOperations.LessThanOrEqual;
                    case "in":
                        return FilterOperations.Contains;
                    case "not_in":
                        return FilterOperations.NotContains;
                    case "sw":
                        return FilterOperations.StartsWith;
                    case "ew":
                        return FilterOperations.EndsWith;
                    case "":
                    default:
                        return FilterOperations.Equals;
                }

            }
        }

        public static bool GetHasAnd(string stringValueUrl)
        {
            return stringValueUrl.EndsWith(":and");
        }

        public static string GetAcroymOperater(FilterOperations type)
        {
            switch (type)
            {
                case FilterOperations.NotEquals:
                    return "neq";
                case FilterOperations.GreaterThan:
                    return "gt";
                case FilterOperations.LessThan:
                    return "lt";
                case FilterOperations.GreaterThanOrEqual:
                    return "gte";
                case FilterOperations.LessThanOrEqual:
                    return "lte";
                case FilterOperations.Contains:
                    return "in";
                case FilterOperations.NotContains:
                    return "not_in";
                case FilterOperations.StartsWith:
                    return "sw";
                case FilterOperations.EndsWith:
                    return "ew";
                case FilterOperations.Equals:
                default:
                    return "eq";
            }
        }

        public static string GetConditionSql(FilterOperations type, object? value)
        {
            switch (type)
            {
                case FilterOperations.GreaterThan:
                    return $"> {QueriesCreatingHelper.GetValue(value)}";
                case FilterOperations.LessThan:
                    return $"< {QueriesCreatingHelper.GetValue(value)}";
                case FilterOperations.GreaterThanOrEqual:
                    return $">= {QueriesCreatingHelper.GetValue(value)}";
                case FilterOperations.LessThanOrEqual:
                    return $"<= {QueriesCreatingHelper.GetValue(value)}";
                case FilterOperations.Contains:
                    return $"like N'%{value?.ToString().PreventSqlInjection()}%'";
                case FilterOperations.NotContains:
                    return $"not like '%{value?.ToString().PreventSqlInjection()}%'";
                case FilterOperations.StartsWith:
                    return $"like N'{value?.ToString().PreventSqlInjection()}%'";
                case FilterOperations.EndsWith:
                    return $"like N'%{value?.ToString().PreventSqlInjection()}'";
                case FilterOperations.NotEquals:
                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                        return $"is not null";
                    return $"<> {QueriesCreatingHelper.GetValue(value)}";
                case FilterOperations.Equals:
                default:
                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                        return $"is null";
                    return $"= {QueriesCreatingHelper.GetValue(value)}";
            }
        }
    }
}
