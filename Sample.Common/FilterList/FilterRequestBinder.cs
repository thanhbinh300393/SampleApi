using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sample.Common.Exceptions;
using Sample.Common.Extentions;

namespace Sample.Common.FilterList
{
    public class FilterRequestBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }


            var modelName = bindingContext.ModelName;

            try
            {
                FilterRequest request = new FilterRequest();

                var query = bindingContext.HttpContext.Request.Query;

                foreach (var item in query)
                {
                    var queryKey = item.Key.Trim() ?? "";
                    var queryValue = item.Value;
                    if (queryKey == "page")
                        request.Page = queryValue.ToInt();
                    else if (queryKey == "limit")
                        request.Limit = queryValue.ToInt();
                    else if (queryKey.StartsWith("f."))
                    {
                        var posColon = queryKey.IndexOf(':');
                        if (posColon < 0) posColon = queryKey.Length;
                        var queryValues = queryValue.ToString().Split('|').Select(x => (object)x).ToList();
                        FilterValue filter = new FilterValue()
                        {
                            PropertyName = queryKey.Substring(2, posColon - 2),
                            Operator = FilterOperationConverter.Get(queryKey.Substring(posColon + 1), true),
                            HasAnd = FilterOperationConverter.GetHasAnd(queryKey.Substring(posColon + 1)),
                            Values = queryValues,
                        };
                        request.Filters.Add(filter);
                    }
                    else if (queryKey == "sort")
                    {
                        var orders = queryValue.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < orders.Length; i++)
                        {
                            var propertyName = (orders[i].StartsWith("-") || orders[i].StartsWith("+")) ? orders[i].Substring(1) : orders[i];
                            OrderValue order = new OrderValue()
                            {
                                PropertyName = propertyName.Trim(),
                                Type = orders[i].StartsWith("-") ? OrderTypes.desc : OrderTypes.asc,
                                Index = i
                            };
                            request.Orders.Add(order);
                        }
                    }
                    else if (queryKey.Equals("full"))
                    {
                        request.IsFull = queryValue.ToString().ToLower().Equals("false") ? false : true;
                    }
                    else if (queryKey.Equals("tree"))
                    {
                        request.Tree = queryValue.ToString().ToLower().Equals("false") ? false : true;
                    }
                    else if (queryKey.Equals("summary"))
                    {
                        request.Summary = queryValue;
                    }
                    else if (queryKey.Equals("q"))
                    {
                        request.Keyword = queryValue;
                    }
                }

                bindingContext.Result = ModelBindingResult.Success(request);
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Tham số lọc không hợp lệ", ex.Message, new { modelName });
            }
            return Task.CompletedTask;

        }
    }
}
