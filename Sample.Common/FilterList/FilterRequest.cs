using Sample.Common.Heplers;

namespace Sample.Common.FilterList
{
    public class FilterRequest
    {
        private int _page = 1;

        public int Page
        {
            get { return _page; }
            set
            {
                _page = value;
                if (value <= 0)
                {
                    _page = 1;
                }
            }
        }

        private int _limit = 10;

        public int Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                if (value <= 0)
                {
                    _limit = 1;
                }
            }
        }

        public bool IsFull { get; set; }

        public bool HasGetCount { get; set; } = true;

        public string? Keyword { get; set; }

        public string? Summary { get; set; }

        public List<string> SummaryFields =>
            string.IsNullOrWhiteSpace(Summary)
                ? new List<string>()
                : Summary.Split(',')
                         .Select(x => x.Trim())
                         .Where(x => !string.IsNullOrWhiteSpace(x))
                         .ToList();


        public bool Tree { get; set; } = false;

        public List<FilterValue> Filters { get; set; }

        public List<OrderValue> Orders { get; set; }

        public FilterRequest()
        {
            Filters = new List<FilterValue>();
            Orders = new List<OrderValue>();
        }

        public List<FilterValue> GetFilters(string fieldName)
        {
            return Filters.Where(x => x.PropertyName?.ToLower() == fieldName.ToLower()).ToList();
        }

        public List<object> GetValueFilters(string fieldName)
        {
            return Filters
                .Where(x => x.PropertyName?.ToLower() == fieldName.ToLower() && x.Operator == FilterOperations.Equals)
                .FirstOrDefault()?.Values ?? new List<object>();
        }

        public object? GetValueFilter(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return null;

            var filter = Filters
                .FirstOrDefault(x =>
                    x.Operator == FilterOperations.Equals &&
                    string.Equals(x.PropertyName, fieldName, StringComparison.OrdinalIgnoreCase)
                );

            return filter?.Values?.FirstOrDefault();
        }

        public static FilterRequest CreateInstance(bool isFull = true)
        {
            return new FilterRequest() { IsFull = isFull };
        }
    }

    public class FilterRequestFromBody : FilterRequest
    {
    }

    public static class FilterRequestHelper
    {
        public static FilterRequest Filter<T>(this FilterRequest filterRequest, string fieldName, T value,
            FilterOperations operation = FilterOperations.Equals, bool hasAnd = false)
        {
            filterRequest.Filters.Add(new FilterValue()
            {
                PropertyName = fieldName,
                Operator = operation,
                HasAnd = hasAnd,
                Values = (new object[] { value.ToString()?.ToLower() == "null" ? null : value }).ToList()
            });

            return filterRequest;
        }

        public static FilterRequest Filters<T>(
            this FilterRequest filterRequest,
            string fieldName,
            IEnumerable<T> values,
            FilterOperations operation = FilterOperations.Equals,
            bool hasAnd = false)
        {
            filterRequest.Filters.Add(new FilterValue()
            {
                PropertyName = fieldName,
                Operator = operation,
                HasAnd = hasAnd,
                Values = values.Select(x => (object)x).ToList()
            });

            return filterRequest;
        }

        public static List<FilterRequest> BreakFilters(
            this FilterRequest filterRequest)
        {
            var response = new List<FilterRequest>();

            var currentQueryLength = 0;
            var maxQueryLength = 864000;

            filterRequest.Filters = filterRequest.Filters.OrderByDescending(x => x.Values.Count).ToList();
            var maxLengthFilterItem = filterRequest.Filters.FirstOrDefault();
            if (maxLengthFilterItem == null)
            {
                return new List<FilterRequest>() { filterRequest };
            }

            var otherFilterItems = filterRequest.Filters.Where(x => x.PropertyName != maxLengthFilterItem.PropertyName).ToList();

            foreach (var filterValue in otherFilterItems)
            {
                foreach (object filterValueValue in filterValue.Values)
                {
                    if (filterValueValue != null)
                    {
                        currentQueryLength += 10 + filterValue.PropertyName.Length + filterValueValue.ToString()!.Length;
                    }
                }
            }

            var remainingLength = maxQueryLength - currentQueryLength;
            if (remainingLength < 0)
            {
                return new List<FilterRequest>() { filterRequest };
            }

            var partOfBreakFilter = MapHelper.Mapper<FilterValue, FilterValue>(maxLengthFilterItem);
            partOfBreakFilter.Values = new List<object>();

            var currentPartLength = 0;
            foreach (object value in maxLengthFilterItem.Values)
            {
                if (currentPartLength > remainingLength)
                {
                    var newResponseItem = MapHelper.Mapper<FilterRequest, FilterRequest>(filterRequest);
                    newResponseItem.Filters = new List<FilterValue>();
                    foreach (var otherFilterItem in otherFilterItems)
                    {
                        newResponseItem.Filters.Add(otherFilterItem);
                    }

                    newResponseItem.Filters.Add(MapHelper.Mapper<FilterValue, FilterValue>(partOfBreakFilter));
                    response.Add(newResponseItem);

                    partOfBreakFilter.Values = new List<object>();
                    currentPartLength = 0;
                }

                currentPartLength += 10 + maxLengthFilterItem.PropertyName.Length + value.ToString()!.Length;
                partOfBreakFilter.Values.Add(value);
            }

            if (!response.Any())
            {
                response.Add(filterRequest);
            }

            return response;
        }
    }
}