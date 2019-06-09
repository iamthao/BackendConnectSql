using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System.Configuration;
using System.Transactions;

namespace ServiceLayer
{
    public class UserService : MasterFileService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        public UserService(ITenantPersistenceService tenantPersistenceService, IUserRepository userRepository,IUserRoleRepository userRoleRepository,IBusinessRuleSet<User> businessRuleSet = null)
            : base(userRepository, userRepository, tenantPersistenceService, businessRuleSet)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public User GetUserByUserNameAndPass(string username, string password)
        {
            var hashedPassword = PasswordHelper.HashString(password, username);
            var user = _userRepository.GetUserByUserNameAndPass(username, hashedPassword);
            return user;
        }

        public User GetUserFromHashStringPasswordAndUsername(string code)
        {
            return _userRepository.GetUserFromHashStringPasswordAndUsername(code);
        }     
       
    }
}