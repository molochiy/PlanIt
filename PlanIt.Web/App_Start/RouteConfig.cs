using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Routing;

namespace PlanIt.Web
{
    [ExcludeFromCodeCoverage]
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(null,
            //    "Page{page}",
            //    new { controller = "Plan", action = "AddPlan", category = (string)null },
            //    new { page = @"\d+" }
            //);

            //routes.MapRoute(null,
            //    "Page{page}",
            //    new { controller = "Plan", action = "Index", category = (string)null },
            //    new { page = @"\d+" }
            //);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
