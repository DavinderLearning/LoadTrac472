using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArTrax472.Startup))]
namespace ArTrax472
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
