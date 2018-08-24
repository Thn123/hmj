using System.Web.Mvc;
using System.Web.Routing;

namespace Hmj.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("assets/{*file}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}.do/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}