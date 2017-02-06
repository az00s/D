using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(D.Startup))]
namespace D
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
