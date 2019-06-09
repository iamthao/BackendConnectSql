using System.Security.Principal;
using Framework.DomainModel.Entities;

namespace Framework.DomainModel.Interfaces
{
    public interface IQuickspatchPrincipal : IPrincipal
    {
        string AuthToken { get; set; }
        new QuickspatchIdentity Identity { get; set; }
        User User { get; set; }
    }
}