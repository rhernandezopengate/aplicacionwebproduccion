using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppProduccion.Startup))]
namespace WebAppProduccion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
