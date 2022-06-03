using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArTrax472b.Startup))]
namespace ArTrax472b
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
