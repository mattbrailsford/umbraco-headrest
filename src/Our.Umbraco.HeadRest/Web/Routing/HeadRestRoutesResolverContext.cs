using Umbraco.Core.Models;
using Umbraco.Web;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    public class HeadRestRoutesResolverContext
    {
        public UmbracoContext UmbracoContext { get; internal set; }
        public IPublishedContent RootContent { get; set; }
    }
}
