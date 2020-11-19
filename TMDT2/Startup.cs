using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TMDT2.Startup))]
namespace TMDT2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
