using System.Collections.Generic;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    public class DefaultHeadRestRoutesResolver : HeadRestRoutesResolver
    {
        public DefaultHeadRestRoutesResolver(string slug) 
            : base(slug)
        { }

        public override void Resolve(ICollection<string> routes, int nodeId, string contentTypeAlias,
            HeadRestRoutesResolverContext context)
        {
            routes.Add(context.UmbracoContext.UrlProvider.GetUrl(nodeId));
        }
    }
}
