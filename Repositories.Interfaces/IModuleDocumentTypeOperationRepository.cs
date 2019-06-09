using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IModuleDocumentTypeOperationRepository : IRepository<ModuleDocumentTypeOperation>, IQueryableRepository<ModuleDocumentTypeOperation>
    {
    }
}