using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ITemplateRepository : IRepository<Template>, IQueryableRepository<Template>
    {
     
    }
}
