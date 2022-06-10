using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoadTrac742.Startup))]
namespace LoadTrac742
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
