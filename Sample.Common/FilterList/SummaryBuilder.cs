using Sample.Common.Extentions;
using Sample.Common.Heplers;
using System.Dynamic;

namespace Sample.Common.FilterList
{
    public static class SummaryBuilder
    {
        public static object Summary<T>(this IEnumerable<T> list, string summaryFields)
        {
            return list.AsQueryable().Summary(summaryFields);
        }

        public static object Summary<T>(this IQueryable<T> source, string summaryFields)
        {
            dynamic summary = new ExpandoObject();
            List<string> fields = summaryFields?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLowerFirstChar()).ToList() ?? new List<string>();
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                if (fields.Find(x => x == prop.Name.ToLowerFirstChar()) != null)
                {
                    ((IDictionary<String, Object>)summary).Add(new KeyValuePair<String, Object>(prop.Name.ToLowerFirstChar(), source.Sum(prop.Name)));
                }
            }

            return summary;
        }
    }
}
