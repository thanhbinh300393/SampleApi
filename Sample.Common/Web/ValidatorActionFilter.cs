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
            ILanguageProvider<CommonResource> l = filterContext.HttpContext.RequestServices.GetService<ILanguageProvider<CommonResource>>();

            if (!filterContext.ModelState.IsValid)
            {
                Dictionary<string, string[]> validates = new Dictionary<string, string[]>();
                foreach (var item in filterContext.ModelState)
                {
                    validates.Add(item.Key, item.Value.Errors.Select(x => l[x.ErrorMessage]).ToArray());
                }
                throw new BadRequestException(l["InputDataIsWrong"], "Input data is wrong", data: validates);
            }
        }
    }

}
