namespace EverlastingStudent.Web
{
    using System.Reflection;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using EverlastingStudent.Common.Infrastructure.Automapper;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var autoMapperConfig = new AutoMapperConfig(
                new[]
                {
                    Assembly.GetExecutingAssembly(),
                    Assembly.Load("EverlastingStudent.DataTransferObjects")
                });
            autoMapperConfig.Execute();
        }
    }
}
