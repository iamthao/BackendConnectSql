using System.Collections.Generic;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer
{
    public class UserRoleService : MasterFileService<UserRole>, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        public UserRoleService(ITenantPersistenceService tenantPersistenceService, IUserRoleRepository userRoleRepository,
            IBusinessRuleSet<UserRole> businessRuleSet = null)
            : base(userRoleRepository, userRoleRepository, tenantPersistenceService, businessRuleSet)
        {
            _userRoleRepository = userRoleRepository;
        }

        public dynamic GetRoleFunction(int idRole)
        {
            return _userRoleRepository.GetRoleFunction(idRole);
        }

        public IEnumerable<int> GetAllDocumentTypeId()
        {
            return _userRoleRepository.GetAllDocumentTypeId();
        }

        
        public List<LookupItemVo> GetUserRoleNoCourier()
        {
            var userrole = _userRoleRepository.GetUserRoleNoCourier();
            return userrole;
        }
        
    }
}