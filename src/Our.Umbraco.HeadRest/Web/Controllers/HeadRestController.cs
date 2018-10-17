using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Our.Umbraco.HeadRest.Web.Mvc;
using Our.Umbraco.HeadRest.Web.Mapping;
using System.Collections.Generic;
using Our.Umbraco.HeadRest.Interfaces;
using Umbraco.Core;
using Our.Umbraco.HeadRest.Web.Models;
using Our.Umbraco.HeadRest.Web.Routing;

namespace Our.Umbraco.HeadRest.Web.Controllers
{
    [HeadRestExceptionFilter]
    public class HeadRestController : RenderMvcController
    {
        internal IHeadRestConfig Config
        {
            get
            {
                return RouteData.Values["headRestConfig"] as IHeadRestConfig;
            }
        }

        public override ActionResult Index(RenderModel model)
        {
            // Check for 404
            if (model.Content is NotFoundPublishedContent)
            {
                Response.StatusCode = 404;
          
                return new HeadRestResult
                {
                    Data = new { message = "404 Not Found" }
                };
            }

            // Check for routes request
            if (Config.RoutesResolver != null && RouteData.Values["path"] != null)
            {
                var path = RouteData.Values["path"].ToString();
                if (Config.RoutesResolver.Slug.Trim('/').InvariantEquals(path.Trim('/')))
                {
                    return Routes(model);
                }
            }

            // Process the model mapping request
            var contentTypeAlias = model.Content.DocumentTypeAlias;
            var viewModelType = Config.ViewModelMappings.GetViewModelTypeFor(contentTypeAlias);
            var viewModel = Config.Mapper.Invoke(new HeadRestMappingContext
            {
                Content = model.Content,
                ContentType = model.Content.GetType(),
                ViewModelType = viewModelType,
                RequestContext = new HeadRestMappingRequestContext
                {
                    Request = Request,
                    HttpContext = HttpContext,
                    UmbracoContext = UmbracoContext
                }
            });

            return new HeadRestResult
            {
                Data = viewModel
            };
        }

        protected virtual ActionResult Routes(RenderModel model)
        {
            var routes = Config.RoutesResolver.ResolveRoutes(new HeadRestRoutesResolverContext
            {
                UmbracoContext = UmbracoContext,
                RootNode = model.Content
            });

            return new HeadRestResult
            {
                Data = routes
            };
        }
    }
}
