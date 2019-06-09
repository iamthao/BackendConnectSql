using Microsoft.Owin.Cors;
using Owin;

namespace QuickSpatchWindowsService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
        }
    }
}
