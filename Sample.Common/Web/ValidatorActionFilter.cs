using Sample.Common.Exceptions;
using Sample.Common.Languages;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Common.Web
{
    public class ValidatorActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var l = filterContext.HttpContext.RequestServices
                .GetRequiredService<ILanguageProvider<CommonResource>>();

            if (!filterContext.ModelState.IsValid)
            {
                var validates = filterContext.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value.Errors
                            .Select(e => l[e.ErrorMessage])
                            .ToArray()
                    );
                throw new BadRequestException(
                    l["InputDataIsWrong"],
                    "Input data is wrong",
                    data: validates
                );
            }
        }
    }

}
