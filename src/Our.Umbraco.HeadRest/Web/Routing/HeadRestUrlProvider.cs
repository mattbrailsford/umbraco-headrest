using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    internal class HeadRestUrlProvider : IUrlProvider
    {
        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
            var content = umbracoContext.ContentCache.GetById(id);

            foreach(var headRestConfig in HeadRest.Configs.Values)
            {
                var rootNode = umbracoContext.ContentCache.GetSingleByXPath(headRestConfig.RootNodeXPath);
                if (content.Path.StartsWith(rootNode.Path))
                {
                    var subUrl = string.Join("/", content.AncestorsOrSelf(true, x => x.Level > rootNode.Level)
                        .Select(x => x.UrlName)
                        .Reverse());

                    var url = (headRestConfig.BasePath
                        .EnsureStartsWith("/")
                        .EnsureEndsWith("/") + subUrl).EnsureEndsWith('/');

                    return url;
                }
            }

            return null;
        }

        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            return null;
        }
    }
}
