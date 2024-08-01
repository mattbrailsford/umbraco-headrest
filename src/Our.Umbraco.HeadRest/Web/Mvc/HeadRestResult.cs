using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Our.Umbraco.HeadRest.Web.Mvc
{
    internal class HeadRestResult : ActionResult
    {
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        /// <summary>
        /// Default, unchanged JsonSerializerSettings
        /// </summary>
        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings();

        public HeadRestResult()
        {
            SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var response = context.HttpContext.Response;

            response.ContentType = "application/json; charset=utf-8";

            if (Data != null)
            {
                using (StreamWriter sw = new StreamWriter(response.Body, new UTF8Encoding(false)))
                using (JsonTextWriter writer = new JsonTextWriter(sw) { Formatting = Formatting })
                {
                    var serializer = JsonSerializer.Create(SerializerSettings);
                    serializer.Serialize(writer, Data);
                    writer.Flush();
                }
            }
        }
    }
}
