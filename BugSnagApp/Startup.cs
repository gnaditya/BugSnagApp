using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugSnagApp.Startup))]
namespace BugSnagApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
