using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Our.Umbraco.HeadRest.Web.Models
{
    /// <summary>
    /// Special instance of IPublishedContent to allow our custom route
    /// to run all the way to our controller, then we can throw a 404
    /// and return JSON content instead of the HTML 404 that would 
    /// ordinarily return HTML
    /// </summary>
    internal class NotFoundPublishedContent : IPublishedContent
    {
        public object this[string alias] => null;

        public IEnumerable<IPublishedContent> ContentSet => Enumerable.Empty<IPublishedContent>();

        public PublishedContentType ContentType => null;

        public int Id => -404;

        public int TemplateId => 0;

        public int SortOrder => 0;

        public string Name => "404 Not Found";

        public string UrlName => null;

        public string DocumentTypeAlias => "404";

        public int DocumentTypeId => 0;

        public string WriterName => null;

        public string CreatorName => null;

        public int WriterId => 0;

        public int CreatorId => 0;

        public string Path => "-404";

        public DateTime CreateDate => DateTime.MinValue;

        public DateTime UpdateDate => DateTime.MinValue;

        public Guid Version => Guid.Empty;

        public int Level => 0;

        public string Url => null;

        public PublishedItemType ItemType => PublishedItemType.Content;

        public bool IsDraft => false;

        public IPublishedContent Parent => null;

        public IEnumerable<IPublishedContent> Children => null;

        public ICollection<IPublishedProperty> Properties => new Collection<IPublishedProperty>();

        public int GetIndex()
        {
            return 0;
        }

        public IPublishedProperty GetProperty(string alias)
        {
            return null;
        }

        public IPublishedProperty GetProperty(string alias, bool recurse)
        {
            return null;
        }
    }
}
