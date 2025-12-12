using Sample.Common.Heplers;

namespace Sample.Common.FilterList
{
    public class FilterResult<T>
        where T : class
    {
        private int _page = 1;

        public int Page
        {
            get
            {
                if (IsFull) return 1;
                return _page;
            }
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
            get
            {
                if (IsFull) return TotalRecords;
                return _limit;
            }
            set
            {
                _limit = value;
                if (value <= 0)
                {
                    _limit = 1;
                }
            }
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalRecords * 1.0 / Limit);
            }
        }

        public List<T> List { get; set; } = new List<T>();
        public int TotalRecords { get; set; }
        public object? Summary { get; set; }
        public bool IsFull { get; set; }

    }

    public class FilterResult
    {
        public static FilterResult<TDto> Pagination<TEntity, TDto>(IEnumerable<TEntity> source,
            FilterRequest request, object? summary = null, Func<TEntity, TDto>? mappper = null)
            where TEntity : class
            where TDto : class
        {
            return Pagination<TEntity, TDto>(source.AsQueryable(), request, summary, mappper);
        }

        public static FilterResult<TDto> Pagination<TEntity, TDto>(IQueryable<TEntity> source,
            FilterRequest request, object? summary = null, Func<TEntity, TDto>? mappper = null)
            where TEntity : class
            where TDto : class
        {
            if (request.IsFull)
            {
                request.Limit = source.Count();
                request.Page = 1;
            }

            if (mappper == null)
                mappper = new Func<TEntity, TDto>(x => MapHelper.Mapper<TEntity, TDto>(x));

            return new FilterResult<TDto>
            {
                IsFull = request.IsFull,
                Limit = request.Limit,
                Page = request.Page,
                TotalRecords = source.Count(),
                List = source
                    .Skip((request.Page - 1) * request.Limit)
                    .Take(request.Limit)
                    .ToList().Select(mappper).ToList(),
                Summary = summary ?? new { }
            };
        }
    }
}
