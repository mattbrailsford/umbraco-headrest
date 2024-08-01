using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Our.Umbraco.HeadRest.Web.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Our.Umbraco.HeadRest.Web.Controllers
{
    public class AuthorizedHeadRestController : HeadRestController
    {
        public AuthorizedHeadRestController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {

        }

        [Authorize]
        public override IActionResult Index()
        {
            return base.Index();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.StatusCode = 401;

            // This doesn't exist in netcore - do we need it?
            // context.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;

            context.Result = new HeadRestResult
            {
                Data = new { Message = "Not authorized" }
            };
        }
    }
}
