using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;

namespace ServiceLayer.Authorization
{
    public interface IOperationAuthorization
    {
        bool VerifyAccess(DocumentTypeKey documentType, OperationAction action, out List<UserRoleFunction> permissionOfThisView);

    }
}
