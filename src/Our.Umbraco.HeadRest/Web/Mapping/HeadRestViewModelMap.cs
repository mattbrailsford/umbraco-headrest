using System;
using System.Collections.Generic;

namespace Our.Umbraco.HeadRest.Web.Mapping
{
    public class HeadRestViewModelMap
    {
        private IList<HeadRestViewModelMapInfo> _modelMaps;
        private HeadRestViewModelMapping _mapping;
        private HeadRestDefaultModelMapping _defaultMapping;
        private Type _defaultViewModelMapType;

        public HeadRestViewModelMap()
        {
            _modelMaps = new List<HeadRestViewModelMapInfo>();
            _mapping = new HeadRestViewModelMapping(this);
            _defaultMapping = new HeadRestDefaultModelMapping(this);
            _defaultViewModelMapType = null;
        }

        public HeadRestViewModelMapping For(string contentTypeAlias)
        {
            _mapping.Reset(new HeadRestViewModelMapInfo(contentTypeAlias));
            return _mapping;
        }

        public HeadRestDefaultModelMapping Default()
        {
            return _defaultMapping;
        }

        internal void AddViewModelMap(HeadRestViewModelMapInfo viewModelMap)
        {
            _modelMaps.Add(viewModelMap);
        }

        internal void SetDefaultViewModelMapType(Type defaultViewModelMapType)
        {
            _defaultViewModelMapType = defaultViewModelMapType;
        }

        internal Type GetViewModelTypeFor(string contentTypeAlias, HeadRestPreMappingContext ctx)
        {
            foreach(var map in _modelMaps)
            {
                if (map.ContentTypeAlias == contentTypeAlias && map.Condition.Invoke(ctx))
                {
                    return map.ViewModelType;
                }
            }

            return _defaultViewModelMapType;
        }
    }

    public class HeadRestViewModelMapping
    {
        private HeadRestViewModelMap _modelMap;

        private HeadRestViewModelMapInfo _mapInfo { get; set; }

        internal HeadRestViewModelMapping(HeadRestViewModelMap modelMap)
        {
            _modelMap = modelMap;
        }

        internal HeadRestViewModelMapping Reset(HeadRestViewModelMapInfo mapInfo)
        {
            _mapInfo = mapInfo;
            return this;
        }

        public HeadRestViewModelMapping If(Func<HeadRestPreMappingContext, bool> condition)
        {
            _mapInfo.Condition = condition;
            return this;
        }

        public HeadRestViewModelMap MapTo<TViewModel>()
            where TViewModel : class
        {
            _mapInfo.ViewModelType = typeof(TViewModel);
            _modelMap.AddViewModelMap(_mapInfo);
            return _modelMap;
        }
    }

    public class HeadRestDefaultModelMapping
    {
        private HeadRestViewModelMap _modelMap;

        internal HeadRestDefaultModelMapping(HeadRestViewModelMap modelMap)
        {
            _modelMap = modelMap;
        }

        public HeadRestViewModelMap MapTo<TViewModel>()
            where TViewModel : class
        {
            _modelMap.SetDefaultViewModelMapType(typeof(TViewModel));
            return _modelMap;
        }
    }

    internal class HeadRestViewModelMapInfo
    {
        public string ContentTypeAlias { get; set; }
        public Func<HeadRestPreMappingContext, bool> Condition { get; set; }
        public Type ViewModelType { get; set; }

        public HeadRestViewModelMapInfo(string contentTypeAlias)
        {
            ContentTypeAlias = contentTypeAlias;
            Condition = (ctx) => true;
        }
    }
}
