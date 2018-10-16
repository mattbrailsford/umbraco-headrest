using System;
using System.Collections.Generic;

namespace Our.Umbraco.HeadRest.Web.Mapping
{
    public class HeadRestViewModelMap
    {
        private IDictionary<string, Type> _viewModels;
        private HeadRestViewModelForMap _for;

        public HeadRestViewModelMap()
        {
            _viewModels = new Dictionary<string, Type>();
            _for = new HeadRestViewModelForMap(this);
        }

        public HeadRestViewModelForMap For(string contentTypeAlias)
        {
            _for.ContentTypeAlias = contentTypeAlias;
            return _for;
        }

        internal void AddViewModelMap(string contentTypeAlias, Type viewModelType)
        {
            _viewModels.Add(contentTypeAlias, viewModelType);
        }

        internal bool HasViewModelTypeFor(string contentTypeAlias)
        {
            return _viewModels.ContainsKey(contentTypeAlias);
        }

        internal Type GetViewModelTypeFor(string contentTypeAlias)
        {
            return HasViewModelTypeFor(contentTypeAlias)
                ? _viewModels[contentTypeAlias]
                : null;
        }
    }

    public class HeadRestViewModelForMap
    {
        private HeadRestViewModelMap _modelMap;

        internal string ContentTypeAlias { get; set; }

        internal HeadRestViewModelForMap(HeadRestViewModelMap modelMap)
        {
            _modelMap = modelMap;
        }

        public HeadRestViewModelMap MapTo<TViewModel>()
            where TViewModel : class
        {
            _modelMap.AddViewModelMap(ContentTypeAlias, typeof(TViewModel));
            return _modelMap;
        }
    }
}
