using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>, IQueryableRepository<User>
    {
        User GetUserByUserNameAndPass(string username, string password);
        User GetUserFromHashStringPasswordAndUsername(string code);
        //
        int AddUserBySqlString(User user, string database);
    }
}