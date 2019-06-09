using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface INoteRequestRepository : IRepository<NoteRequest>, IQueryableRepository<NoteRequest>
    {
        List<NoteRequestDetail> GetNotesDetail(int requestId);
    }
}
