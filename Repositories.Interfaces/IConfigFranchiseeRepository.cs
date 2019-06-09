using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IConfigFranchiseeRepository : IRepository<ConfigFranchisee>, IQueryableRepository<ConfigFranchisee>
    {
    }
}