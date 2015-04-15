using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using EverlastingStudent.Web;

using Microsoft.Owin;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(Startup))]

namespace EverlastingStudent.Web
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            this.ConfigureAuth(app);
        }
    }
}
