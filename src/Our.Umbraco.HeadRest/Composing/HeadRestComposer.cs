using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.HeadRest.Mapping;
using Our.Umbraco.HeadRest.Web.Routing;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Routing;

namespace Our.Umbraco.HeadRest.Composing
{
    public class HeadRestComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.WithCollectionBuilder<UrlProviderCollectionBuilder>()
                .InsertBefore<DefaultUrlProvider, HeadRestUrlProvider>();

            builder.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
                .Add<HeadRestMapDefinition>();

            builder.Services.AddTransient<HeadRest>();
        }
    }
}
