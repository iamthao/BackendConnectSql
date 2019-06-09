using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web.Mvc;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Utility;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.User;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using QuickspatchWeb.Services.Interface;
using Framework.Exceptions;
using Newtonsoft.Json;
using QuickspatchWeb.Models.FranchiseeConfiguration;

namespace QuickspatchWeb.Controllers
{
    public class UserController : ApplicationControllerGeneric<User, DashboardUserDataViewModel>
    {
        private readonly IUserService _userService;
        private readonly IGridConfigService _gridConfigService;
        private readonly IRenderViewToString _renderViewToString;
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        private readonly List<string> _imageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
        private readonly IEmailHandler _emailHandler;
        private readonly IResizeImage _resizeImage;
        private readonly IWebApiConsumeUserService _webApiConsumeUserService;

        public UserController(IAuthenticationService authenticationService, IRenderViewToString renderViewToString, IDiagnosticService diagnosticService,
             IResizeImage resizeImage,IGridConfigService gridConfigService, IUserService userService,
            IFranchiseeConfigurationService franchiseeConfigurationService, IWebApiConsumeUserService webApiConsumeUserService)
            : base(authenticationService, diagnosticService, userService)
        {
            _userService = userService;
            _gridConfigService = gridConfigService;
            _renderViewToString = renderViewToString;
            _franchiseeConfigurationService = franchiseeConfigurationService;
            _emailHandler = new EmailHandler();
            _resizeImage = resizeImage;
            _webApiConsumeUserService = webApiConsumeUserService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var viewModel = new DashboardUserIndexViewModel();
            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "UserGrid",
                ModelName = "User",
                DocumentTypeId = (int)DocumentTypeKey.User,
                GridInternalName = "User",
                UseDeleteColumn = true,
                CanResetPasswordRecord = true,
                PopupWidth = 800,
                PopupHeight = 350
            };

            viewModel.GridViewModel = BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public JsonResult GetDataForGrid(UserQueryInfo queryInfo)
        {
            var user = AuthenticationService.GetCurrentUser().User;
            if (user != null)
            {
                queryInfo.CurrentUserId = user.Id;
            }
            var queryData = _userService.GetDataForGridMasterfile(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);

            return clientsJson;
        }

        protected override IList<ViewColumnViewModel> GetViewColumns()
        {
            return new List<ViewColumnViewModel>
           
            {
                new ViewColumnViewModel
                {
                    ColumnOrder = 2,
                    ColumnWidth = 100,
                    Name = "UserName",
                    Text = "Username",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 3,
                    ColumnWidth = 100,
                    Name = "FullName",
                    Text = "Name",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 4,
                    ColumnWidth = 80,
                    Name = "Role",
                    Text = "Role",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 5,
                    ColumnWidth = 125,
                    Name = "Email",
                    Text = "Email",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 6,
                    ColumnWidth = 80,
                    Name = "HomePhoneInFormat",
                    Text = "Home Phone",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 7,
                    ColumnWidth = 80,
                    Name = "MobilePhoneInFormat",
                    Text = "Mobile Phone",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 8,
                    ColumnWidth = 80,
                    Name = "IsActive",
                    Text = "Active",
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate = "userTemplate"
                }              
            };

        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.Add)]
        public ActionResult Create()
        {
            var viewModel = new DashboardUserDataViewModel
            {
                SharedViewModel = new DashboardUserShareViewModel
                {
                    CreateMode = true,
                    IsActive = true
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.Add)]
        public int Create(UserParameter parameters)
        {
            //get franchisee configuration
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var packageInfo = _webApiConsumeUserService.GetListPackageChange(objFranchiseeAndLicense);
          
            //Bat dau Add
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<User>();
            // Generate password
            const string randomPassword = "123456";
            if (entity.UserName != null)
            {
                entity.Password = PasswordHelper.HashString(randomPassword, entity.UserName);
            }
            //entity.Password = "123456";
            var sharViewModel = viewModel.SharedViewModel as DashboardUserShareViewModel;//
            var logoFilePath = "";
            if (sharViewModel != null)
            {
                if (!String.IsNullOrEmpty(sharViewModel.Avatar))
                {
                    if (!sharViewModel.Avatar.Contains("data:image/jpg;base64"))
                    {
                        logoFilePath = Server.MapPath(sharViewModel.Avatar);
                        entity.Avatar = _resizeImage.ResizeImageByHeightAndWidth(logoFilePath, 450, 450);
                    }
                }
            }


            var savedEntity = MasterFileService.Add(entity);
            if (savedEntity.Id > 0)
            {
                var webLink = AppSettingsReader.GetValue("Url", typeof(String)) as string;
                var urlSignIn = webLink + "/Authentication/SignIn";
                var imgSrc = webLink + "/Content/quickspatch/img/logo-o.svg";
                var urlChangePass = webLink + "/Authentication/ChangePassword?code=" + PasswordHelper.HashString(savedEntity.Id.ToString(), entity.UserName);
                var fromEmail = AppSettingsReader.GetValue("EmailFrom", typeof(String)) as string;
                var displayName = AppSettingsReader.GetValue("EmailFromDisplayName", typeof(String)) as string;
                //var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                var franchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "";
                var emailContent = Framework.Utility.TemplateHelpper.FormatTemplateWithContentTemplate(
                    TemplateHelpper.ReadContentFromFile(TemplateConfigFile.CreateUserEmailTemplate, true),
                    new
                    {
                        img_src = imgSrc,
                        full_name = Framework.Utility.CaculatorHelper.GetFullName(savedEntity.FirstName, savedEntity.MiddleName, savedEntity.LastName),
                        web_link = webLink,
                        user_name = entity.UserName,
                        password = randomPassword,
                        url_change_pass = urlChangePass,
                        franchisee_Name = franchiseeName,
                        url_sign_in = urlSignIn
                    });
                // send email
                var logo = franchiseeConfiguration != null ? franchiseeConfiguration.Logo : new byte[0];
                _emailHandler.SendEmailSsl(fromEmail, new[] { savedEntity.Email }, SystemMessageLookup.GetMessage("SubjectToSendEmailForCreateUser"),
                                        emailContent, logo, true, displayName);
            }
            return savedEntity.Id;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Update)]
        public ActionResult Update(UserParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            var logoFilePath = "";

            if (ModelState.IsValid)
            {
                var sharViewModel = viewModel.SharedViewModel as DashboardUserShareViewModel;

                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);


                if (sharViewModel != null)
                {
                    if (!String.IsNullOrEmpty(sharViewModel.Avatar))
                    {
                        if (!sharViewModel.Avatar.Contains("data:image/jpg;base64"))
                        {
                            logoFilePath = Server.MapPath(sharViewModel.Avatar);
                            mappedEntity.Avatar = _resizeImage.ResizeImageByHeightAndWidth(logoFilePath, 450, 450);
                        }
                    }

                }

                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.View)]
        public ActionResult GetLookup(LookupQuery queryInfo)
        {
            var selector = new Func<User, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = CaculatorHelper.GetFullName(o.FirstName, o.MiddleName, o.LastName)
            });
            return base.GetLookupForEntity(queryInfo, selector);

        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {
            if (AuthenticationService.GetCurrentUser().User.Id == id)
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("CannotDeleteYourself"));
                MasterFileService.ThrowCustomValidation(mess);
            }

            MasterFileService.DeleteById(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportExcel(List<ColumnModel> gridColumns, UserQueryInfo queryInfo)
        {
            var user = AuthenticationService.GetCurrentUser().User;
            if (user != null)
            {
                queryInfo.CurrentUserId = user.Id;
            }
            return base.ExportExcelMasterfile(gridColumns, queryInfo);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.View)]
        public FileResult DownloadExcelFile(string fileName)
        {
            return base.DownloadExcelMasterFile(fileName);
        }


        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.Update)]
        public ActionResult ResetPassword(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.Update)]
        public ActionResult ResetPassword(int id, string password, string username)
        {
            var user = _userService.GetById(id);
            if (user != null)
            {
                string randomPassword = password;
                user.Password = PasswordHelper.HashString(randomPassword, user.UserName);
                _userService.Update(user);

                var webLink = AppSettingsReader.GetValue("Url", typeof(String)) as string;
                var urlSignIn = webLink + "/Authentication/SignIn";
                var imgSrc = webLink + "/Content/quickspatch/img/logo-o.svg";
                var urlChangePass = webLink + "/Authentication/ChangePassword?code=" + PasswordHelper.HashString(id.ToString(), user.UserName);
                var fromEmail = AppSettingsReader.GetValue("EmailFrom", typeof(String)) as string;
                var displayName = AppSettingsReader.GetValue("EmailFromDisplayName", typeof(String)) as string;
                var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                var franchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "";
                var emailContent = TemplateHelpper.FormatTemplateWithContentTemplate(
                    TemplateHelpper.ReadContentFromFile(TemplateConfigFile.RestorePasswordTemplate, true),
                    new
                    {
                        img_src = imgSrc,
                        full_name = Framework.Utility.CaculatorHelper.GetFullName(user.FirstName, user.MiddleName, user.LastName),
                        web_link = webLink,
                        user_name = user.UserName,
                        password = randomPassword,
                        url_change_pass = urlChangePass,
                        franchisee_Name = franchiseeName,
                        url_sign_in = urlSignIn
                    });
                // send email
                var logo = franchiseeConfiguration != null ? franchiseeConfiguration.Logo : new byte[0];
                _emailHandler.SendEmailSsl(fromEmail, new[] { user.Email }, SystemMessageLookup.GetMessage("SubjectToSendEmailForCreateUser"),
                                        emailContent,logo, true, displayName);
            }


            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public ActionResult UserProfile()
        {
            var currentUser = _userService.GetById(AuthenticationService.GetCurrentUser().User.Id);
            //var viewModel = currentUser.MapTo<DashboardUserDataViewModel>();
            string avatar = " ";
            if (currentUser.Avatar != null)
            {
                avatar = "data:image/jpg;base64," + Convert.ToBase64String(currentUser.Avatar);
            }
            else
            {
                avatar = "/Content/quickspatch/img/avatar.png";
            }

            var viewModel = new DashboardUserProfileViewModel
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                MiddleName = currentUser.MiddleName,
                UserRoleName = currentUser.UserRole.Name,
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                HomePhone = currentUser.HomePhone.ApplyFormatPhone(),
                MobilePhone = currentUser.MobilePhone.ApplyFormatPhone(),
                Avatar = avatar,
            };

            return View(viewModel);
        }

        //passwword Profile
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None)]
        public ActionResult ChangePasswordProfile()
        {
            return PartialView("_ChangePasswordProfile");
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None)]
        public JsonResult SaveChangePasswordProfile(string parameters)
        {
            var viewModel = JsonConvert.DeserializeObject<PasswordViewModel>(parameters);

            var currentUser = AuthenticationService.GetCurrentUser().User;
            var user = _userService.GetById(currentUser.Id);
            if (user.Password != PasswordHelper.HashString(viewModel.CurrentPassword, user.UserName))
            {
                return Json(new { Status = false },
                    JsonRequestBehavior.AllowGet);
            }
            user.Password = PasswordHelper.HashString(viewModel.Password, user.UserName);
            var saveId = _userService.Update(user);
            return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
        }
        //update Profile
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public ActionResult UpdateProfile()
        {
            var currentUser = _userService.GetById(AuthenticationService.GetCurrentUser().User.Id);
            var viewModel = new DashboardUserProfileViewModel
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                UserRoleName = currentUser.UserRole.Name,
                UserRoleId = currentUser.UserRoleId,
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                MiddleName = currentUser.MiddleName,
                HomePhone = currentUser.HomePhone.ApplyFormatPhone(),
                MobilePhone = currentUser.MobilePhone.ApplyFormatPhone(),
                LastModified = currentUser.LastModified,
                IsActive = currentUser.IsActive,
                Password = currentUser.Password

            };

            return View(viewModel);
        }

        //edit avatar
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public ActionResult EditAvatar()
        {
            var currentUser = _userService.GetById(AuthenticationService.GetCurrentUser().User.Id);
            string avatar = " ";
            if (currentUser.Avatar != null)
            {
                avatar = "data:image/jpg;base64," + Convert.ToBase64String(currentUser.Avatar);
            }
            else
            {
                avatar = "/Content/quickspatch/img/avatar.png";
            }
            var viewModel = new DashboardUserProfileViewModel
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                UserRoleName = currentUser.UserRole.Name,
                UserRoleId = currentUser.UserRoleId,
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                MiddleName = currentUser.MiddleName,
                HomePhone = currentUser.HomePhone.ApplyFormatPhone(),
                MobilePhone = currentUser.MobilePhone.ApplyFormatPhone(),
                LastModified = currentUser.LastModified,
                IsActive = currentUser.IsActive,
                Password = currentUser.Password,
                Avatar = avatar,
            };

            return View(viewModel);
        }
     
    }
}