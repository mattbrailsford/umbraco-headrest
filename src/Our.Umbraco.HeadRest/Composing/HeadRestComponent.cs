using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace Our.Umbraco.HeadRest.Composing
{
    public class HeadRestComponent : IComponent
    {
        public void Initialize()
        {
            RouteTable.Routes.IgnoreStandardExclusions();
        }

        public void Terminate()
        { }
    }
}
