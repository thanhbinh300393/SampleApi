namespace Sample.Common.FilterList
{
    public static class OrderBuilder
    {
        public static IOrderedEnumerable<TSource> OrderByIf<TSource>(this IEnumerable<TSource> source, bool condition, IList<OrderValue> orderValues)
        {
            if (condition)
                return source.OrderBy(orderValues);
            else
                return source.OrderBy(x => 1);
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> list, IList<OrderValue> orderValues)
        {
            var props = typeof(TSource).GetProperties();

            if (orderValues == null || orderValues.Count <= 0)
            {
                orderValues = new List<OrderValue>
                {
                    new OrderValue
                    {
                        PropertyName = props[0].Name,
                        Type = OrderTypes.asc,
                        Index = 0
                    }
                };
            }
            orderValues = orderValues.OrderBy(x => x.Index).ToList();

            IOrderedEnumerable<TSource>? result = null;
            List<string> builder = new List<string>();
            foreach (var order in orderValues)
            {
                var prop = props.FirstOrDefault(p => string.Equals(p.Name, order.PropertyName, StringComparison.OrdinalIgnoreCase));
                if (prop == null) continue;

                Func<TSource, object?> keySelector = item => prop.GetValue(item);

                if (result == null)
                {
                    result = order.Type == OrderTypes.asc
                        ? list.OrderBy(keySelector)
                        : list.OrderByDescending(keySelector);
                }
                else
                {
                    result = order.Type == OrderTypes.asc
                        ? result.ThenBy(keySelector)
                        : result.ThenByDescending(keySelector);
                }
            }

            return result ?? list.OrderBy(x => 1);
        }
    }
}
