using System;

namespace Our.Umbraco.HeadRest.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewModelForAttribute : Attribute
    {
        public string ContentTypeAlias { get; set; }

        public ViewModelForAttribute(string contentTypeAlias)
        {
            this.ContentTypeAlias = contentTypeAlias;
        }
    }
}
