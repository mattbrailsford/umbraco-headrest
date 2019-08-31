using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Core.Models.PublishedContent;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    internal class HeadRestUrlProvider : DefaultUrlProvider
    {
        public HeadRestUrlProvider(IRequestHandlerSection requestSettings, ILogger logger, 
            IGlobalSettings globalSettings, ISiteDomainHelper siteDomainHelper)
            : base(requestSettings, logger, globalSettings, siteDomainHelper)
        { }

        public override UrlInfo GetUrl(UmbracoContext umbracoContext, IPublishedContent content, UrlMode mode, string culture, Uri current)
        {
            var headRestUrl = GetHeadRestUrl(umbracoContext, content.Id, culture, HeadRestEndpointMode.Dedicated);

            return headRestUrl ?? base.GetUrl(umbracoContext, content, mode, culture, current);
        }

        public override IEnumerable<UrlInfo> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            var headRestUrl = GetHeadRestUrl(umbracoContext, id, null, HeadRestEndpointMode.Mixed);

            return headRestUrl != null ? new[] { headRestUrl } : base.GetOtherUrls(umbracoContext, id, current);
        }

        protected UrlInfo GetHeadRestUrl(UmbracoContext umbracoContext, int id, string culture, HeadRestEndpointMode endpointMode)
        {
            var content = umbracoContext.Content.GetById(id);

            foreach (var headRestConfig in HeadRest.Configs.Values.Where(x => x.Mode == endpointMode))
            {
                var rootNode = umbracoContext.Content.GetSingleByXPath(headRestConfig.RootNodeXPath);
                if (content.Path.StartsWith(rootNode.Path))
                {
                    var subUrl = string.Join("/", content.AncestorsOrSelf(true, x => x.Level > rootNode.Level)
                        .Select(x => x.UrlSegment)
                        .Reverse());

                    var url = (headRestConfig.BasePath
                        .EnsureStartsWith("/")
                        .EnsureEndsWith("/") + subUrl).EnsureEndsWith('/');

                    return UrlInfo.Url(url, culture); ;
                }
            }

            return null;
        }
    }
}
