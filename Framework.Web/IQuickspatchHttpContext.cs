using System.Security.Principal;
using System.Web;

namespace Framework.Web
{
    public interface IQuickspatchHttpContext
    {
        HttpContextBase Context { get; }
        HttpRequestBase Request { get; }
        HttpResponseBase Response { get; }
        IPrincipal User { get; set; }
    }
}
