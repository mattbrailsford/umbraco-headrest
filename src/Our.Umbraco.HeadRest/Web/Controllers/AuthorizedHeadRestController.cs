using System.Web.Mvc;
using Our.Umbraco.HeadRest.Web.Mvc;
using Umbraco.Web.Models;

namespace Our.Umbraco.HeadRest.Web.Controllers
{
    public class AuthorizedHeadRestController : HeadRestController
    {
        [HeadRestAuthorize]
        public override ActionResult Index(RenderModel model)
        {
            return base.Index(model);
        }
    }
}
