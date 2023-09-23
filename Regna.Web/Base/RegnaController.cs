using Microsoft.AspNetCore.Mvc;

namespace Regna.Web.Base
{
    public class RegnaController : Controller
    {
            protected string ServiceUrl { get; }

            public RegnaController()
            {
            ServiceUrl = "http://localhost:6969";
            }
    }
}
