using System.Collections.Generic;
using System.Text.RegularExpressions;
using Umbraco.Core;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    public class HeadRestRouteMap
    {
        private IList<HeadRestRouteMapInfo> _routeMaps;
        private HeadRestRouteMapping _mapping;

        public HeadRestRouteMap()
        {
            _routeMaps = new List<HeadRestRouteMapInfo>();
            _mapping = new HeadRestRouteMapping(this);
        }

        public HeadRestRouteMapping For(string pattern)
        {
            _mapping.Reset(new HeadRestRouteMapInfo(pattern));
            return _mapping;
        }

        internal void AddRouteMap(HeadRestRouteMapInfo routeMap)
        {
            _routeMaps.Add(routeMap);
        }

        internal HeadRestRouteMapMatch GetRouteMapFor(string path)
        {
            if (path == null)
                return null;

            path = path.EnsureStartsWith("/");

            foreach (var map in _routeMaps)
            {
                var match = Regex.Match(path, map.Pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return new HeadRestRouteMapMatch
                    {
                        Target = match.Result(map.Target),
                        Params = match.Groups
                    };
                }
            }
            return null;
        }
    }

    public class HeadRestRouteMapping
    {
        private HeadRestRouteMap _routeMap;
        private HeadRestRouteMapInfo _mapInfo;

        internal HeadRestRouteMapping(HeadRestRouteMap routeMap)
        {
            _routeMap = routeMap;
        }

        internal HeadRestRouteMapping Reset(HeadRestRouteMapInfo mapInfo)
        {
            _mapInfo = mapInfo;
            return this;
        }

        public HeadRestRouteMap MapTo(string target)
        {
            _mapInfo.Target = target;
            _routeMap.AddRouteMap(_mapInfo);
            return _routeMap;
        }
    }

    public class HeadRestRouteMapInfo
    {
        public string Pattern { get; set; }
        public string Target { get; set; }

        public HeadRestRouteMapInfo(string pattern)
        {
            Pattern = pattern;
        }
    }

    public class HeadRestRouteMapMatch
    {
        public string Target { get; set; }
        public HeadRestRouteParamsCollection Params { get; set; }
    }
}
