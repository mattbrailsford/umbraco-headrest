using System.Text.RegularExpressions;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    public class HeadRestRouteParamsCollection
    {
        private GroupCollection _groups;

        internal HeadRestRouteParamsCollection(GroupCollection groups)
        {
            _groups = groups;
        }

        public string this[int index] 
        {
            get
            {
                var result = _groups[index];
                return result != null ? result.Value : null;
            }
        }

        public string this[string name]
        {
            get
            {
                var result = _groups[name];
                return result != null ? result.Value : null;
            }
        }

        public static implicit operator HeadRestRouteParamsCollection(GroupCollection groups)
        {
            return new HeadRestRouteParamsCollection(groups);
        }
    }
}
