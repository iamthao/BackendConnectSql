using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IContactRepository : IRepository<Contact>, IQueryableRepository<Contact>
    {
        List<ContactGridVo> GetListContact();
    }
}
