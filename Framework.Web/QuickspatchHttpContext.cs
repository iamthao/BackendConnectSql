using System.Security.Principal;
using System.Web;

namespace Framework.Web
{
    public class QuickspatchHttpContext : IQuickspatchHttpContext
    {
        public HttpContextBase Context
        {
            get { return new HttpContextWrapper(HttpContext.Current); }
        }

        public HttpRequestBase Request
        {
            get { return Context.Request; }
        }

        public HttpResponseBase Response
        {
            get { return Context.Response; }
        }

        public IPrincipal User
        {
            get { return Context.User; }
            set { Context.User = value; }
        }
    }
}