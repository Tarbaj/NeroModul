using DotNetNuke.Web.Api;

namespace picturpictur
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute(
                moduleFolderName:   "pictur",
                routeName:          "default",
                url:                "{controller}/{action}",
                defaults:           new { },
                namespaces:         new[] {"picturpictur.Controllers"});
        }
    }
}