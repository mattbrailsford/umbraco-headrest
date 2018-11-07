using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    internal class HeadRestUrlProvider : DefaultUrlProvider
	{
		public HeadRestUrlProvider(IRequestHandlerSection requestSettings)
			: base(requestSettings)
		{ }

        public override string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
			var headRestUrl = GetHeadRestUrl(umbracoContext, id, HeadRestEndpointMode.Dedicated);

			return headRestUrl ?? base.GetUrl(umbracoContext, id, current, mode);
        }

        public override IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
			var headRestUrl = GetHeadRestUrl(umbracoContext, id, HeadRestEndpointMode.Mixed);

			return headRestUrl != null ? new[] { headRestUrl } : base.GetOtherUrls(umbracoContext, id, current);
        }

		protected string GetHeadRestUrl(UmbracoContext umbracoContext, int id, HeadRestEndpointMode endpointMode)
		{
			var content = umbracoContext.ContentCache.GetById(id);

			foreach (var headRestConfig in HeadRest.Configs.Values.Where(x => x.Mode == endpointMode))
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
    }
}
