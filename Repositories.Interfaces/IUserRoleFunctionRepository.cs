using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IUserRoleFunctionRepository : IRepository<UserRoleFunction>, IQueryableRepository<UserRoleFunction>
    {
        List<UserRoleFunction> LoadUserSecurityRoleFunction(int userRoleId, int documentTypeId);
    }
}