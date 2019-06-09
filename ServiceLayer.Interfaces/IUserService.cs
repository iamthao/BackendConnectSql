using Framework.DomainModel.Entities;

namespace ServiceLayer.Interfaces
{
    public interface IUserService : IMasterFileService<User>
    {
        User GetUserByUserNameAndPass(string username, string password);
        User GetUserFromHashStringPasswordAndUsername(string code);
        
    }
}