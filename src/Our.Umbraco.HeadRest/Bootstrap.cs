using AutoMapper;
using Umbraco.Core;
using Our.Umbraco.HeadRest.Mapping;
using Our.Umbraco.HeadRest.Web.Resolvers;
using Umbraco.Web.Routing;
using Our.Umbraco.HeadRest.Web.Routing;

namespace Our.Umbraco.HeadRest
{
    internal class Bootstrap : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            InitResolvers();
            InitAutoMapper();
            InitRouting();
        }

        private void InitResolvers()
        {
            ViewModelsResolver.Current = ViewModelsResolver.Create(PluginManager.Current);
        }

        private void InitAutoMapper()
        {
            Mapper.AddProfile<HeadRestMappingProfile>();
        }

        private void InitRouting()
        {
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, HeadRestUrlProvider>();
        }
    }
}
