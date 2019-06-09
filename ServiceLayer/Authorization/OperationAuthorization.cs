using System.Collections.Generic;
using System.Linq;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.Interfaces;
using Framework.Web;
using Repositories.Interfaces;
using ServiceLayer.Common;

namespace ServiceLayer.Authorization
{
    public class OperationAuthorization : IOperationAuthorization
    {
        private readonly IQuickspatchHttpContext _oxProjectContext;
        private readonly IUserRoleFunctionRepository _userRoleFunctionRepostory;

        public OperationAuthorization(IQuickspatchHttpContext oxProjectContext,
                                      IUserRoleFunctionRepository userRoleFunctionRepostory)
        {
            _oxProjectContext = oxProjectContext;
            _userRoleFunctionRepostory = userRoleFunctionRepostory;
        }
        public bool VerifyAccess(DocumentTypeKey documentType, OperationAction action,
                                       out List<UserRoleFunction> permissionOfThisView)
        {
            var principal = _oxProjectContext.User as IQuickspatchPrincipal;
            var hasPermission = false;
            permissionOfThisView = null;

            if (principal != null && principal.User != null)
            {
                var userRoleId = principal.User.UserRoleId.GetValueOrDefault();

                var userGroupRights = MenuExtractData.Instance.LoadUserSecurityRoleFunction(userRoleId,
                    (int)documentType);
                if (userGroupRights == null || userGroupRights.Count==0)
                {
                    userGroupRights = _userRoleFunctionRepostory.LoadUserSecurityRoleFunction(userRoleId, (int)documentType);
                }

                if (userGroupRights != null)
                {
                    hasPermission = userGroupRights.Any(r => r.SecurityOperationId == (int)action);
                    permissionOfThisView = userGroupRights.ToList();
                }

            }

            return hasPermission;
        }
    }
}