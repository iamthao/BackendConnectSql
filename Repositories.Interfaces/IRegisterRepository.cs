using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IRegisterRepository : IRepository<Register>, IQueryableRepository<Register>
    {
    }
}