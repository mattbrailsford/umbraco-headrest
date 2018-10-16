using System;
using System.Collections.Generic;

namespace Our.Umbraco.HeadRest.Web.Mapping
{
    public class HeadRestViewModelMap
    {
        private IDictionary<string, Type> _viewModels;

        public HeadRestViewModelMap()
        {
            _viewModels = new Dictionary<string, Type>();
        }

        public HeadRestViewModelForMap For(string contentTypeAlias)
        {
            return new HeadRestViewModelForMap(this, contentTypeAlias);
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
        private string _contentTypeAlias;

        internal HeadRestViewModelForMap(HeadRestViewModelMap modelMap, string contentTypeAlias)
        {
            _modelMap = modelMap;
            _contentTypeAlias = contentTypeAlias;
        }

        public HeadRestViewModelMap MapTo<TViewModel>()
            where TViewModel : class
        {
            _modelMap.AddViewModelMap(_contentTypeAlias, typeof(TViewModel));
            return _modelMap;
        }
    }
}
