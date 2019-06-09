using System;
using System.Linq;
using System.Transactions;
using ConfigValues;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.ValueObject;
using Framework.Service.Translation;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System.Collections.Generic;
using System.Transactions;
using System.ComponentModel.DataAnnotations;
using Framework.DomainModel.Entities.Common;

namespace ServiceLayer
{
    public class FranchiseeTenantService : MasterFileService<FranchiseeTenant>, IFranchiseeTenantService
    {
        private readonly IFranchiseeTenantRepository _franchiseeTenantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IFranchiseeConfigurationRepository _franchiseeConfigurationRepository;


        public FranchiseeTenantService(ITenantPersistenceService tenantPersistenceService, IFranchiseeTenantRepository franchiseeTenantRepository,
            IUserRepository userRepository, IUserRoleRepository userRoleRepository, IFranchiseeConfigurationRepository franchiseeConfigurationRepository,

            IBusinessRuleSet<FranchiseeTenant> businessRuleSet = null)
            : base(franchiseeTenantRepository, franchiseeTenantRepository, tenantPersistenceService, businessRuleSet)
        {
            _franchiseeTenantRepository = franchiseeTenantRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _franchiseeConfigurationRepository = franchiseeConfigurationRepository;

        }

        public FranchiseeTenant CheckFranchiseeWithNameAndLicenseKey(string franchiseeName, string licenseKey)
        {
            var now = DateTime.UtcNow;
            return
                _franchiseeTenantRepository.FirstOrDefault(
                    o => o.Name == franchiseeName && o.LicenseKey == licenseKey && o.EndActiveDate >= now && o.IsActive);
        }

        public ActiveDateLicenseKeyDto GetActiveDateLicenseKey(FranchisseNameAndLicenseDto franchiseeData)
        {
            return _franchiseeTenantRepository.GetActiveDateLicenseKey(franchiseeData);
        }

        public ModuleForFranchiseeDto GetModuleForFranchisee(FranchisseNameAndLicenseDto franchiseeData)
        {
            return _franchiseeTenantRepository.GetModuleForFranchisee(franchiseeData);
        }

        public bool UpdateFranchiseeConfig(FranchiseeTernantDto franchiseeData)
        {
            FranchiseeTenant franchiseeTenant = null;
            var query = _franchiseeTenantRepository.Get(o => true).ToList();
            if (query.Any())
            {
                foreach (var o in query)
                {
                    if (o != null)
                    {
                        var hashCode = PasswordHelper.HashString(o.Id.ToString(), o.Name);
                        if (hashCode == franchiseeData.FranchiseeId)
                        {
                            franchiseeTenant = o;
                            break;

                        }
                    }
                }
            }
            if (franchiseeTenant != null)
            {
                franchiseeTenant.Address1 = franchiseeData.Address1;
                franchiseeTenant.Address2 = franchiseeData.Address2;
                franchiseeTenant.City = franchiseeData.City;
                franchiseeTenant.State = franchiseeData.State;
                franchiseeTenant.Zip = franchiseeData.Zip;
                franchiseeTenant.OfficePhone = franchiseeData.OfficePhone;
                franchiseeTenant.FaxNumber = franchiseeData.FaxNumber;
                franchiseeTenant.IndustryId = franchiseeData.IndustryId;

                Update(franchiseeTenant);
                return true;
            }
            return false;
        }

        public int SetupFranchisee(FranchiseeTenant franchiseeTenant, FranchiseeConfiguration franchiseeConfiguration)
        {
            using (var scope = new TransactionScope())
            {
                //Add Franchise
                ValidateBusinessRules(franchiseeTenant);
                _franchiseeTenantRepository.Add(franchiseeTenant);
                _franchiseeTenantRepository.Commit();
                int i = 0;
                var connectionString = PersistenceHelper.GenerateConnectionString(franchiseeTenant.Server,
                franchiseeTenant.Database, franchiseeTenant.UserName, franchiseeTenant.Password);
                var database = franchiseeTenant.Database;

                // Create franchisee configuaration
                // Check franchisee configuaration has exists
                #region
                _franchiseeConfigurationRepository.DeleteAllFranchiseeConfigurationBySqlString(database);

                var franchiseeId = PasswordHelper.HashString(franchiseeTenant.Id.ToString(), franchiseeTenant.Name);
                var licenseKey = franchiseeTenant.LicenseKey;
                var franchiseeContact = franchiseeConfiguration.FranchiseeContact;
                var primaryContactPhone = franchiseeConfiguration.PrimaryContactPhone;
                var primaryContactEmail = franchiseeConfiguration.PrimaryContactEmail;
                var primaryContactFax = franchiseeConfiguration.PrimaryContactFax;
                var primaryContactCellNumber = franchiseeConfiguration.PrimaryContactCellNumber;
                var name = franchiseeConfiguration.Name;
                var city = franchiseeConfiguration.City;
                var state = franchiseeConfiguration.State;
                var zip = franchiseeConfiguration.Zip;
                var address1 = franchiseeConfiguration.Address1;
                var address2 = franchiseeConfiguration.Address2;
                var officePhone = franchiseeConfiguration.OfficePhone;
                var faxNumber = franchiseeConfiguration.FaxNumber;
                var industryId = franchiseeConfiguration.IndustryId;
                var logo = franchiseeConfiguration.Logo;

                var franchiseeconfig = new FranchiseeConfiguration
                {
                    FranchiseeId = franchiseeId,
                    LicenseKey = licenseKey,
                    FranchiseeContact = franchiseeContact,
                    PrimaryContactPhone = primaryContactPhone,
                    PrimaryContactEmail = primaryContactEmail,
                    PrimaryContactFax = primaryContactFax,
                    PrimaryContactCellNumber = primaryContactCellNumber,
                    Name = name,
                    City = city,
                    State = state,
                    Zip = zip,
                    Address1 = address1,
                    Address2 = address2,
                    OfficePhone = officePhone,
                    FaxNumber = faxNumber,
                    Logo = logo,
                    IndustryId = industryId
                };
                _franchiseeConfigurationRepository.AddFranchiseeConfigurationBySqlString(franchiseeconfig, database);
                #endregion

                // Create a franchisee admin role
                #region

                _userRoleRepository.ChangeConnectionString(connectionString);

                var franchiseeAdminRole = _userRoleRepository.FirstOrDefault(o => o.AppRoleName == AppRole.GlobalAdmin.ToString());
                var idFranchiseeAdminRole = 0;
                if (franchiseeAdminRole == null)
                {
                    // Create franchisee admin role
                    var franchiseeAdminRoleAdd = new UserRole
                    {
                        Name = "Franchisee Admin",
                        AppRoleName = AppRole.GlobalAdmin.ToString(),
                        UserRoleFunctions = new List<UserRoleFunction>()
                    };
                    // Create list userRoleFunction for franchisee admin
                    var objListDocumentType = _userRoleRepository.GetAllDocumentType();
                    foreach (var documentType in objListDocumentType)
                    {
                        var objViewAdd = new UserRoleFunction
                        {
                            DocumentTypeId = documentType.Id,
                            SecurityOperationId = (int)OperationAction.View
                        };
                        franchiseeAdminRoleAdd.UserRoleFunctions.Add(objViewAdd);

                        //Implement View insert
                        var objInsertAdd = new UserRoleFunction
                        {
                            DocumentTypeId = documentType.Id,
                            SecurityOperationId = (int)OperationAction.Add,
                        };
                        franchiseeAdminRoleAdd.UserRoleFunctions.Add(objInsertAdd);

                        //Implement View update
                        var objUpdateAdd = new UserRoleFunction
                        {
                            DocumentTypeId = documentType.Id,
                            SecurityOperationId = (int)OperationAction.Update,
                        };
                        franchiseeAdminRoleAdd.UserRoleFunctions.Add(objUpdateAdd);
                        //Implement View delete
                        var objDeleteAdd = new UserRoleFunction
                        {
                            DocumentTypeId = documentType.Id,
                            SecurityOperationId = (int)OperationAction.Delete,
                        };
                        franchiseeAdminRoleAdd.UserRoleFunctions.Add(objDeleteAdd);

                        var objProcessAdd = new UserRoleFunction
                        {
                            DocumentTypeId = documentType.Id,
                            SecurityOperationId = (int)OperationAction.Process,
                        };
                        franchiseeAdminRoleAdd.UserRoleFunctions.Add(objProcessAdd);
                    }
                    _userRoleRepository.Add(franchiseeAdminRoleAdd);
                    _userRoleRepository.Commit();
                    idFranchiseeAdminRole = franchiseeAdminRoleAdd.Id;
                }
                else
                {
                    idFranchiseeAdminRole = franchiseeAdminRole.Id;
                }
                #endregion

                // Create franchisee admin user
                // Check user admin has exists
                #region
                _userRepository.ChangeConnectionString(connectionString);
                var isExistsUserFranchiseeAdmin = _userRepository.CheckExist(o => o.UserRoleId == idFranchiseeAdminRole);
                if (!isExistsUserFranchiseeAdmin)
                {
                    var randomPassword = "123456";
                    string username = franchiseeTenant.Name.Replace(" ", "");
                    var password = PasswordHelper.HashString(randomPassword, username);
                    var phoneNumber = franchiseeConfiguration.PrimaryContactPhone;// "1111111111";
                    var email = franchiseeConfiguration.PrimaryContactEmail;//  "franchisee_admin@caminois.com";              
                    var cellPhone = franchiseeConfiguration.PrimaryContactCellNumber;// "1111111111";

                    // Create user franchisee admin
                    var userFranchiseeAdmin = new User
                    {
                        UserName = username,
                        Password = password,
                        UserRoleId = idFranchiseeAdminRole,
                        IsActive = true,
                        FirstName = "Admin",
                        LastName = "Franchisee",
                        HomePhone = phoneNumber,
                        MobilePhone = cellPhone,
                        Email = email,

                    };
                    _userRepository.AddUserBySqlString(userFranchiseeAdmin, database);
                }
                #endregion
                scope.Complete();
                return franchiseeTenant.Id;
            }
        }

        public void GetFranchiseeDataForUpdate(int labId, out FranchiseeTenant outfranchiseeTenant,
            out FranchiseeConfiguration outfranchiseeConfiguration)
        {
            outfranchiseeTenant = null;
            outfranchiseeConfiguration = null;

            outfranchiseeTenant = _franchiseeTenantRepository.GetById(labId);
            if (outfranchiseeTenant != null)
            {
                // change the connection
                var connectionString = PersistenceHelper.GenerateConnectionString(outfranchiseeTenant.Server, outfranchiseeTenant.Database, outfranchiseeTenant.UserName, outfranchiseeTenant.Password);
                _franchiseeConfigurationRepository.ChangeConnectionString(connectionString);
                outfranchiseeConfiguration = _franchiseeConfigurationRepository.FirstOrDefault();
            }
        }

        public byte[] UpdateFranchisee(FranchiseeTenant franchiseeTenant,
            FranchiseeConfiguration franchiseeConfiguration)
        {
            using (var scope = new TransactionScope())
            {
                ValidateBusinessRules(franchiseeTenant);
                _franchiseeTenantRepository.Update(franchiseeTenant);
                _franchiseeTenantRepository.Commit();

                // change the connection
                var connectionString = PersistenceHelper.GenerateConnectionString(franchiseeTenant.Server, franchiseeTenant.Database, franchiseeTenant.UserName, franchiseeTenant.Password);
                var database = franchiseeTenant.Database;

                _franchiseeConfigurationRepository.ChangeConnectionString(connectionString);
                _franchiseeConfigurationRepository.DeleteAllFranchiseeConfigurationBySqlString(database);


                // Create franchisee configuaration
                //Add new Franchisee Configuration
                var franchiseeId = PasswordHelper.HashString(franchiseeTenant.Id.ToString(), franchiseeTenant.Name);
                var licenseKey = franchiseeTenant.LicenseKey;
                var franchiseeContact = franchiseeConfiguration.FranchiseeContact;
                var primaryContactPhone = franchiseeConfiguration.PrimaryContactPhone;
                var primaryContactEmail = franchiseeConfiguration.PrimaryContactEmail;
                var primaryContactFax = franchiseeConfiguration.PrimaryContactFax;
                var primaryContactCellNumber = franchiseeConfiguration.PrimaryContactCellNumber;
                var name = franchiseeConfiguration.Name;
                var city = franchiseeConfiguration.City;
                var state = franchiseeConfiguration.State;
                var zip = franchiseeConfiguration.Zip;
                var address1 = franchiseeConfiguration.Address1;
                var address2 = franchiseeConfiguration.Address2;
                var officePhone = franchiseeConfiguration.OfficePhone;
                var faxNumber = franchiseeConfiguration.FaxNumber;
                var logo = franchiseeConfiguration.Logo;
                var industryId = franchiseeConfiguration.IndustryId;

                var franchiseeconfig = new FranchiseeConfiguration
                {
                    FranchiseeId = franchiseeId,
                    LicenseKey = licenseKey,
                    FranchiseeContact = franchiseeContact,
                    PrimaryContactPhone = primaryContactPhone,
                    PrimaryContactEmail = primaryContactEmail,
                    PrimaryContactFax = primaryContactFax,
                    PrimaryContactCellNumber = primaryContactCellNumber,
                    Name = name,
                    City = city,
                    State = state,
                    Zip = zip,
                    Address1 = address1,
                    Address2 = address2,
                    OfficePhone = officePhone,
                    FaxNumber = faxNumber,
                    Logo = logo,
                    IndustryId = industryId
                };
                _franchiseeConfigurationRepository.AddFranchiseeConfigurationBySqlString(franchiseeconfig, database);

                scope.Complete();
                return franchiseeTenant.LastModified;
            }
        }

        public bool CheckFranchiseIsExpire(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchisee =
                _franchiseeTenantRepository.FirstOrDefault(
                    o => o.LicenseKey == franchiseeData.LicenseKey && o.Name == franchiseeData.FranchiseeName);
            if (franchisee != null)
            {
                if (franchisee.EndActiveDate.GetValueOrDefault() > DateTime.UtcNow)
                    return true;
            }
            return false;

        }

        public string GetDisplayNameForCourier()
        {
            if (ConstantValue.DeploymentMode == DeploymentMode.Camino)
            {
                return "Mobile User";
            }

            //var config = _franchiseeConfigurationRepository.FirstOrDefault();
            return "Mobile User"; //config.IndustryId == null ? "Courier" : config.Industry.DisplayLabel;
        }

        

        public FranchiseeTenant DeactivateFranchisee(int id)
        {
            var franchisee = _franchiseeTenantRepository.GetById(id);
            franchisee.IsActive = false;
            _franchiseeTenantRepository.Update(franchisee);
            _franchiseeTenantRepository.Commit();
            return franchisee;
        }

        public FranchiseeTenant ActivateFranchisee(int id)
        {
            var franchisee = _franchiseeTenantRepository.GetById(id);
            franchisee.IsActive = true;
            _franchiseeTenantRepository.Update(franchisee);
            _franchiseeTenantRepository.Commit();
            return franchisee;
        }

        public List<LookupItemVo> GetListIndustry()
        {
            return _franchiseeTenantRepository.GetListIndustry();
        }



        public FranchiseeTernantDto GetInfoFranchisee(FranchisseNameAndLicenseDto franchiseeData)
        {
            return _franchiseeTenantRepository.GetInfoFranchisee(franchiseeData);
        }

        public bool UpdateFranchiseeTenantCloseAccount(FranchiseeTernantCloseAccountDto franchiseeData)
        {
            var tenent =
                _franchiseeTenantRepository.FirstOrDefault(
                    o =>
                        o.Name.ToLower().Equals(franchiseeData.FranchiseeName.ToLower()) && o.LicenseKey.Equals(franchiseeData.FranchiseeLicense));
            if (tenent != null)
            {
                tenent.CloseDate = DateTime.UtcNow;
                tenent.DescriptionClose = franchiseeData.Description;
                tenent.QuestionClose = franchiseeData.Question;
                _franchiseeTenantRepository.Update(tenent);
                _franchiseeTenantRepository.Commit();
                return true;
            }
            return false;
        }

        public bool UpdateFranchiseeTenantCancelAccount(FranchisseNameAndLicenseDto franchiseeData)
        {
            var tenent =
                _franchiseeTenantRepository.FirstOrDefault(
                    o =>
                        o.Name.ToLower().Equals(franchiseeData.FranchiseeName.ToLower()) && o.LicenseKey.Equals(franchiseeData.LicenseKey));
            if (tenent != null)
            {
                tenent.CloseDate = null;
                tenent.DescriptionClose = null;
                tenent.QuestionClose = null;
                _franchiseeTenantRepository.Update(tenent);
                _franchiseeTenantRepository.Commit();
                return true;
            }
            return false;
        }


        public FranchiseeTernantCurrentPackageDto GetPackageCurrentId(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchiseeTenant =
                _franchiseeTenantRepository.FirstOrDefault(
                    o => o.LicenseKey.Equals(franchiseeData.LicenseKey) && o.Name.Equals(franchiseeData.FranchiseeName));
            if (franchiseeData != null)
            {
                return new FranchiseeTernantCurrentPackageDto
                {
                    Id = franchiseeTenant.Id,
                    PackageId = franchiseeTenant.CurrentPackageId.GetValueOrDefault(),
                    Active = franchiseeTenant.IsActive && franchiseeTenant.EndActiveDate.GetValueOrDefault() >= DateTime.UtcNow,
                    AccountNumber = franchiseeTenant.AccountNumber,
                    Amount = franchiseeTenant.RemainingAmount
                };
            }
            return null;
        }


        public bool UpdateFranchiseeTenantLicenceExtentsion(FranchisseNameAndLicenseDto franchiseeData)
        {
            var franchiseeTenant =
                _franchiseeTenantRepository.FirstOrDefault(
                    o => o.LicenseKey.Equals(franchiseeData.LicenseKey) && o.Name.Equals(franchiseeData.FranchiseeName));
            if (franchiseeTenant != null)
            {
                //franchiseeTenant.StartActiveDate = DateTime.UtcNow;
                if (franchiseeTenant.CurrentPackageId % 2 == 0)
                {
                    franchiseeTenant.EndActiveDate = DateTime.UtcNow.AddMonths(1);
                }
                else
                {
                    franchiseeTenant.EndActiveDate = DateTime.UtcNow.AddMonths(12);
                }
                franchiseeTenant.CloseDate = null;
                franchiseeTenant.DescriptionClose = null;
                franchiseeTenant.QuestionClose = null;
                franchiseeTenant.StartDateSuccess = null;
                franchiseeTenant.EndDateSuccess = null;
                _franchiseeTenantRepository.Update(franchiseeTenant);
                franchiseeTenant.IsActive = true;
                _franchiseeTenantRepository.Commit();
                return true;
            }
            return false;
        }

        public FranchiseeTernantDto GetInfoFranchiseeNoToken(FranchisseNameAndLicenseDto franchiseeData)
        {
            return _franchiseeTenantRepository.GetInfoFranchisee(franchiseeData);
        }

        public bool FranchiseeTenantUpdatePayment(FranchiseeTenantUpdatePaymentDto franchiseeTenantUpdatePaymentDto)
        {
            var franchisee =
                _franchiseeTenantRepository.FirstOrDefault(
                    o => o.Name == franchiseeTenantUpdatePaymentDto.FranchiseeName
                         && o.LicenseKey == franchiseeTenantUpdatePaymentDto.LicenseKey);

            using (var scope = new TransactionScope())
            {
                franchisee.RemainingAmount = franchiseeTenantUpdatePaymentDto.Amount;
                franchiseeTenantUpdatePaymentDto.NextBillingDate = franchiseeTenantUpdatePaymentDto.NextBillingDate;

                _franchiseeTenantRepository.Update(franchisee);
                _franchiseeTenantRepository.Commit();
                scope.Complete();
                return true;
            }
        }
    }
}