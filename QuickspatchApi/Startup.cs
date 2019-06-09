using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(QuickspatchApi.Startup))]
namespace QuickspatchApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}