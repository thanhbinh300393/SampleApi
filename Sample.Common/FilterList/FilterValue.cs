namespace Sample.Common.FilterList
{
    public class FilterValue
    {
        public string PropertyName { get; set; } = string.Empty;
        public List<object> Values { get; set; } = new List<object>();
        public FilterOperations Operator { get; set; }
        public bool HasAnd { get; set; }
        public bool HasTransform { get; set; } // compare igmore uppercase, lowwecase, capitalize
        public FilterValue()
        {
        }

        public FilterValue(string propertyName, object value, FilterOperations filterOperations = FilterOperations.Equals)
        {
            PropertyName = propertyName;
            Operator = filterOperations;
            Values = new List<object>
            {
                value
            };
        }
    }
}
