using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface IUserRoleService : IMasterFileService<UserRole>
    {
        dynamic GetRoleFunction(int idRole);

        IEnumerable<int> GetAllDocumentTypeId();
        
        List<LookupItemVo> GetUserRoleNoCourier();
    }
}