using System;
using System.Collections.Generic;
using Umbraco.Core;
using Our.Umbraco.HeadRest.Attributes;

namespace Our.Umbraco.HeadRest.Web.Resolvers
{
    internal class ViewModelsResolver
    {
        private readonly Dictionary<string, Type> _viewModelsLookup;

        private ViewModelsResolver()
        {
            _viewModelsLookup = new Dictionary<string, Type>();
        }

        public bool HasViewModelTypeFor(string contentTypeAlias)
        {
            return _viewModelsLookup.ContainsKey(contentTypeAlias);
        }

        public Type GetViewModelTypeFor(string contentTypeAlias)
        {
            return HasViewModelTypeFor(contentTypeAlias)
                ? _viewModelsLookup[contentTypeAlias]
                : null;
        }

        private void Initialize(IEnumerable<Type> types)
        {
            if (types != null)
            {
                foreach (var type in types)
                {
                    var attribute = type.GetCustomAttribute<ViewModelForAttribute>(false);
                    if (attribute == null)
                        continue;

                    if (string.IsNullOrWhiteSpace(attribute.ContentTypeAlias) == false && _viewModelsLookup.ContainsKey(attribute.ContentTypeAlias) == false)
                    {
                        _viewModelsLookup.Add(attribute.ContentTypeAlias, type);
                    }
                }
            }
        }

        public static ViewModelsResolver Current { get; set; }

        public static ViewModelsResolver Create(PluginManager pluginManager)
        {
            return Create(pluginManager.ResolveAttributedTypes<ViewModelForAttribute>());
        }

        public static ViewModelsResolver Create(IEnumerable<Type> types)
        {
            var resolver = new ViewModelsResolver();
            resolver.Initialize(types);
            return resolver;
        }
    }
}
