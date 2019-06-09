using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IDocumentTypeRepository : IRepository<DocumentType>, IQueryableRepository<DocumentType>
    {
    }
}