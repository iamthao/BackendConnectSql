
using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using ServiceLayer.Interfaces;

namespace ServiceLayer.Interfaces
{
    public interface INoteRequestService : IMasterFileService<NoteRequest>
    {
    }
}
