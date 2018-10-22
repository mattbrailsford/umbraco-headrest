using AutoMapper;
using Umbraco.Core;
using Our.Umbraco.HeadRest.Mapping;
using Umbraco.Web.Routing;
using Our.Umbraco.HeadRest.Web.Routing;
using System.Web.Routing;
using Umbraco.Web;

namespace Our.Umbraco.HeadRest
{
    internal class Bootstrap : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            InitAutoMapper();
            InitRouting();
        }

        private void InitAutoMapper()
        {
            Mapper.AddProfile<HeadRestMappingProfile>();
        }

        private void InitRouting()
        { 
            RouteTable.Routes.IgnoreStandardExclusions();
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, HeadRestUrlProvider>();
        }
    }
}
