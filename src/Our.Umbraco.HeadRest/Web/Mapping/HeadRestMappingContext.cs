using System;
using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace Our.Umbraco.HeadRest.Web.Mapping
{
    public class HeadRestMappingContext : HeadRestPreMappingContext
    {
        public Type ViewModelType { get; internal set; }

        internal HeadRestMappingContext()
        { }
    }

    public class HeadRestPreMappingContext
    {
        public IPublishedContent Content { get; internal set; }
        public Type ContentType { get; internal set; }

        public HttpRequest Request { get; internal set; }
        public HttpContext HttpContext { get; internal set; }
        public IUmbracoContext UmbracoContext { get; internal set; }

        internal HeadRestPreMappingContext()
        { }
    }
}
