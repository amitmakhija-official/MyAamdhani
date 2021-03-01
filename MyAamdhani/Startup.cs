using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyAamdhani.Startup))]
namespace MyAamdhani
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
