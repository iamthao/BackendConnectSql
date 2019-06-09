using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;

namespace ServiceLayer.Common
{
    public class MenuExtractData
    {
        public static MenuExtractData Instance
        {
            get { return Nested._instance; }
        }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly MenuExtractData _instance = new MenuExtractData();
        }
        public ModuleForFranchiseeDto ModuleForFranchisee { get; set; }
        public int? NumberOfCourier { get; set; }
        private readonly IUserRoleFunctionRepository _userRoleFunctionRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        private IList<DocumentType> _listDocumentType = new List<DocumentType>();

        private IList<UserRoleFunction> _listUserRoleFunction = new List<UserRoleFunction>();

        private IList<UserRole> _listUserRoles = new List<UserRole>();

        private MenuExtractData()
        {
            _userRoleFunctionRepository = DependencyResolver.Current.GetService<IUserRoleFunctionRepository>();
            _userRoleRepository = DependencyResolver.Current.GetService<IUserRoleRepository>();
            RefershListData();
        }

        public List<UserRoleFunction> LoadUserSecurityRoleFunction(long userRoleId, long documentTypeId)
        {
            if (_listDocumentType.Count == 0)
            {
                _listDocumentType = _userRoleRepository.GetAllDocumentType();
            }
            if (_listUserRoleFunction.Count == 0)
            {
                _listUserRoleFunction = _userRoleFunctionRepository.ListAll();
            }
            var result = (from urf in _listUserRoleFunction
                          join document in _listDocumentType
                              on urf.DocumentTypeId equals document.Id into temp
                          from docType in temp
                          where docType.Id == documentTypeId
                          && urf.UserRoleId == userRoleId
                          select urf);
            return result.ToList();
        }

        public void RefershListData()
        {
            _listDocumentType = new List<DocumentType>();
            _listUserRoleFunction = new List<UserRoleFunction>();
            _listUserRoles = new List<UserRole>();
            if (_listDocumentType.Count == 0)
            {
                _listDocumentType = _userRoleRepository.GetAllDocumentType();
            }
            if (_listUserRoleFunction.Count == 0)
            {
                _listUserRoleFunction = _userRoleFunctionRepository.ListAll();
            }
            if (_listUserRoles.Count == 0)
            {
                _listUserRoles = _userRoleRepository.ListAll();
            }
        }

        public List<UserRole> GetListUserRole()
        {
            return _listUserRoles.ToList();
        }

        private bool CheckUserRoleForDocumentType(int idRole, DocumentTypeKey documentType, OperationAction action)
        {
            var deploymentMode = ConfigurationManager.AppSettings["DeploymentMode"];

            var role = _userRoleRepository.GetById(idRole);
            if (deploymentMode == "franchisee"
                && !string.IsNullOrEmpty(role.AppRoleName) && role.AppRoleName.Equals("GlobalAdmin", StringComparison.InvariantCultureIgnoreCase)
                && _listUserRoleFunction.Any(p => p.DocumentTypeId == (int)documentType && p.DocumentType.Type.Equals(deploymentMode, StringComparison.CurrentCultureIgnoreCase)))
            {
                return true;
            }
                

            if (_listDocumentType.Count == 0 || _listUserRoleFunction.Count == 0)
            {
                _listUserRoleFunction = _userRoleFunctionRepository.ListAll();
                _listDocumentType = _userRoleRepository.GetAllDocumentType();
            }
            return (from urf in _listUserRoleFunction
                    join document in _listDocumentType.Where(p => p.Type.Equals(deploymentMode, StringComparison.CurrentCultureIgnoreCase)) on urf.DocumentTypeId equals document.Id into temp
                    from docType in temp
                    where docType.Id == (int)documentType && urf.UserRoleId == idRole && urf.SecurityOperationId == (int)action
                    select urf).Any();
        }

        private bool CheckModuleFranchisee(int idRole, DocumentTypeKey documentType)
        {
            var deploymentMode = ConfigurationManager.AppSettings["DeploymentMode"];
            var role = _userRoleRepository.GetById(idRole);
            if (deploymentMode == "camino"
                || (deploymentMode == "franchisee" && !string.IsNullOrEmpty(role.AppRoleName)
                && role.AppRoleName.Equals("GlobalAdmin", StringComparison.InvariantCultureIgnoreCase))) return true;

            var listDocumentTypeAvailable = ModuleForFranchisee.ListModuleDocumentTypeOperations.Select(p => p.DocumentTypeId).Distinct();
            return listDocumentTypeAvailable.Any(p => p.Equals((int)documentType));
        }

        public MenuViewModel GetMenuViewModel(int idRole)
        {
            var objResult = new MenuViewModel
            {
                CanViewCourier = 
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Courier, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.Courier),
                CanViewDashboard = 
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Dashboard, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.Dashboard),
                CanViewUserSetup =
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.User),

                CanViewUserRoleSetup =
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.UserRole, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.UserRole, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.UserRole, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.UserRole, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.UserRole),

                CanViewModuleSetup =
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Module, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Module, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Module, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Module, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.Module),

                CanViewFranchiseeSetup =
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.FranchiseeTenant, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.FranchiseeTenant, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.FranchiseeTenant, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.FranchiseeTenant, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.FranchiseeTenant),

                CanViewRequest =
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Request, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Request, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Request, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Request, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.Request),

                CanViewSchedule =
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Schedule, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Schedule, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Schedule, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Schedule, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.Schedule),

                CanViewTracking =
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Tracking, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Tracking, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Tracking, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Tracking, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.Tracking),

                CanViewLocation =
                    CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Location, OperationAction.View)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Location, OperationAction.Add)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Location, OperationAction.Update)
                    //&& CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Location, OperationAction.Delete)
                    && CheckModuleFranchisee(idRole, DocumentTypeKey.Location),

            };
            return objResult;
        }
    }

}
