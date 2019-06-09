using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IUserRoleRepository : IRepository<UserRole>, IQueryableRepository<UserRole>
    {
        dynamic GetRoleFunction(int idRole);

        IEnumerable<int> GetAllDocumentTypeId();

        List<DocumentType> GetAllDocumentType();

        List<LookupItemVo> GetUserRoleNoCourier();


    }
}