using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Our.Umbraco.HeadRest.Web.Mvc;
using Our.Umbraco.HeadRest.Web.Resolvers;
using Our.Umbraco.HeadRest.Web.Mapping;
using System.Collections.Generic;
using Our.Umbraco.HeadRest.Interfaces;
using Umbraco.Core;
using Our.Umbraco.HeadRest.Web.Models;

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
            if (!string.IsNullOrWhiteSpace(Config.RoutesListPath) && RouteData.Values["path"] != null)
            {
                var path = RouteData.Values["path"].ToString();
                if (Config.RoutesListPath.Trim('/').InvariantEquals(path.Trim('/')))
                {
                    return Routes(model);
                }
            }

            // Process the model mapping request
            var contentTypeAlias = model.Content.DocumentTypeAlias;
            var viewModelType = ViewModelsResolver.Current.GetViewModelTypeFor(contentTypeAlias);
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
            var navigator = UmbracoContext.ContentCache.GetXPathNavigator();
            var itterator = navigator.Select($"id({model.Content.Id})/descendant-or-self::*[@isDoc]");

            var routes = new List<string>();

            while (itterator.MoveNext())
            {
                if (int.TryParse(itterator.Current.Evaluate("string(@id)").ToString(), out int id))
                {
                    routes.Add(UmbracoContext.UrlProvider.GetUrl(id));
                }
            }

            return new HeadRestResult
            {
                Data = routes
            };
        }
    }
}
