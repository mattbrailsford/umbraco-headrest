using System.Web.Mvc;

namespace Our.Umbraco.HeadRest.Web.Mvc
{
    internal class HeadRestAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = 401;
            filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            filterContext.Result = new HeadRestResult
            {
                Data = new { Message = "Not authorized" }
            };
        }
    }
}
