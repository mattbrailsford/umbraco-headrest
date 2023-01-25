using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    internal class HeadRestUrlProvider : IUrlProvider
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly DefaultUrlProvider _defaultUrlProvider;

        public HeadRestUrlProvider(IUmbracoContextAccessor umbracoContextAccessor, DefaultUrlProvider defaultUrlProvider)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
            _defaultUrlProvider = defaultUrlProvider;
        }

        public UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current)
        {
            var headRestUrl = GetHeadRestUrl(content.Id, culture, HeadRestEndpointMode.Dedicated);

            return headRestUrl ?? _defaultUrlProvider.GetUrl(content, mode, culture, current);
        }

        public IEnumerable<UrlInfo> GetOtherUrls(int id, Uri current)
        {
            var headRestUrl = GetHeadRestUrl(id, null, HeadRestEndpointMode.Mixed);

            return headRestUrl != null ? new[] { headRestUrl } : _defaultUrlProvider.GetOtherUrls(id, current);
        }

        protected UrlInfo GetHeadRestUrl(int id, string culture, HeadRestEndpointMode endpointMode)
        {
            var umbracoContext = _umbracoContextAccessor.GetRequiredUmbracoContext();

            var content = umbracoContext.Content.GetById(id);

            foreach (var headRestConfig in HeadRest.Configs.Values.Where(x => x.Mode == endpointMode))
            {
                var rootNode = umbracoContext.Content.GetSingleByXPath(headRestConfig.RootNodeXPath);

                if (rootNode == null)
                    continue;

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
