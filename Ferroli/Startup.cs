using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ferroli.Startup))]
namespace Ferroli
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
