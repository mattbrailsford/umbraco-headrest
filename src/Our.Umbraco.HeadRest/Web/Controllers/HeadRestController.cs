using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Our.Umbraco.HeadRest.Interfaces;
using Our.Umbraco.HeadRest.Web.Mapping;
using Our.Umbraco.HeadRest.Web.Models;
using Our.Umbraco.HeadRest.Web.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.Routing;
using Umbraco.Extensions;

namespace Our.Umbraco.HeadRest.Web.Controllers
{
    [TypeFilter(typeof(HeadRestExceptionFilter))]
    public class HeadRestController : UmbracoPageController
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public HeadRestController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
        }

        internal IHeadRestConfig Config
        {
            get
            {
                return RouteData.Values[HeadRest.ControllerConfigKey] as IHeadRestConfig;
            }
        }

        public virtual IActionResult Index()
        {
            var content = CurrentPage;

            // Check for 404
            if (content is NotFoundPublishedContent)
            {
                Response.StatusCode = 404;
          
                return new HeadRestResult
                {
                    Data = new { message = "404 Not Found" }
                };
            }

            // Process the model mapping request
            var umbracoContext = _umbracoContextAccessor.GetRequiredUmbracoContext();
            var contentTypeAlias = content.ContentType.Alias;
            var viewModelType = Config.ViewModelMappings.GetViewModelTypeFor(contentTypeAlias, new HeadRestPreMappingContext
            {
                Request = Request,
                HttpContext = HttpContext,
                UmbracoContext = umbracoContext
            });

            if (viewModelType == null)
                throw new InvalidOperationException($"No view model map found for type '{contentTypeAlias}' at path {Request.Path}");

            var viewModel = Config.Mapper.Invoke(new HeadRestMappingContext
            {
                Content = content,
                ContentType = content.GetType(),
                ViewModelType = viewModelType,
                Request = Request,
                HttpContext = HttpContext,
                UmbracoContext = umbracoContext
            });

            return new HeadRestResult
            {
                Data = viewModel
            };
        }
    }
}
