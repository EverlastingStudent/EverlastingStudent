using EverlastingStudent.Web;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace EverlastingStudent.Web
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
