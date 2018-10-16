using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Umbraco.Web.Mvc;

namespace Our.Umbraco.HeadRest.Web.Mvc
{
    internal class HeadRestResult : JsonNetResult
    {
        public HeadRestResult()
        {
            SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
