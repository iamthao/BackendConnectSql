using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IFranchiseeModuleRepository : IRepository<FranchiseeModule>, IQueryableRepository<FranchiseeModule>
    {
    }
}