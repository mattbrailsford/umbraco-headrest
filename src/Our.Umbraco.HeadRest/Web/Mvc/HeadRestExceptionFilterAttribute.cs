using System.Web.Mvc;

namespace Our.Umbraco.HeadRest.Web.Mvc
{
    internal class HeadRestExceptionFilterAttribute : FilterAttribute, IExceptionFilter
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
