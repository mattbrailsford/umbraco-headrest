using Our.Umbraco.HeadRest.Mapping;
using Our.Umbraco.HeadRest.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Mapping;
using Umbraco.Web.Routing;

namespace Our.Umbraco.HeadRest.Composing
{
    public class HeadRestComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.WithCollectionBuilder<UrlProviderCollectionBuilder>()
                .InsertBefore<DefaultUrlProvider, HeadRestUrlProvider>();

            composition.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
                .Add<HeadRestMapDefinition>();

            composition.Register<HeadRest>();

            composition.Components()
                .Append<HeadRestComponent>();
        }
    }
}
