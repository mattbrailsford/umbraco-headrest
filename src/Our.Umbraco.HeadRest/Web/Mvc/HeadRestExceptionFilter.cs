using Microsoft.AspNetCore.Mvc.Filters;

namespace Our.Umbraco.HeadRest.Web.Mvc
{
    internal class HeadRestExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.ExceptionHandled = true;
            filterContext.Result = new HeadRestResult
            {
                Data = new { message = filterContext.Exception.Message }
            };
        }
    }
}
