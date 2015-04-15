using EverlastingStudent.Web;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace EverlastingStudent.Web
{
    using System.Web.Http;

    using Microsoft.Owin.Cors;

    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
            this.ConfigureAuth(app);
        }
    }
}
