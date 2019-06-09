using System.Security.Principal;

namespace Framework.DomainModel.Interfaces
{
    public interface IQuickspatchIdentity : IIdentity
    {
        int UserIdentityId { get; }
    }
}