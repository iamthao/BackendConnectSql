using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.Common;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Utility;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models.Courier;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using QuickspatchWeb.Models;
using Framework.Service.Translation;
using QuickspatchWeb.Services.Interface;
using QuickspatchWeb.Models.User;

namespace QuickspatchWeb.Controllers
{
    public class CourierController : ApplicationControllerGeneric<Courier, DashboardCourierDataViewModel>
    {
        // GET: Courier

        private readonly ICourierService _courierService;
        private readonly IGridConfigService _gridConfigService;
        private readonly IUserService _userService;
        private readonly IRenderViewToString _renderViewToString;
        private readonly IEmailHandler _emailHandler;
        private readonly IFranchiseeConfigurationService _franchiseeConfigurationService;
        private readonly IResizeImage _resizeImage;
        private readonly ITrackingService _trackingService;
        private readonly IWebApiConsumeUserService _webApiConsumeUserService;

        public CourierController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService, IRenderViewToString renderViewToString,
            IResizeImage resizeImage,IGridConfigService gridConfigService, ICourierService courierService, IUserService userService,
            IFranchiseeConfigurationService franchiseeConfigurationService, ITrackingService trackingService, IWebApiConsumeUserService webApiConsumeUserService)
            : base(authenticationService, diagnosticService, courierService)
        {
            _courierService = courierService;
            _gridConfigService = gridConfigService;
            _userService = userService;
            _renderViewToString = renderViewToString;
            _franchiseeConfigurationService = franchiseeConfigurationService;
            _emailHandler = new EmailHandler();
            _resizeImage = resizeImage;
            _trackingService = trackingService;
            _webApiConsumeUserService = webApiConsumeUserService;
        }
        //index
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.View)]
        public ActionResult Index()
        {
            var viewModel = new DashboardCourierIndexViewModel();
            Func<GridViewModel> gridViewModel = () => new GridViewModel
            {
                GridId = "CourierGrid",
                ModelName = "Courier",
                DocumentTypeId = (int)DocumentTypeKey.Courier,
                GridInternalName = "Courier",
                UseDeleteColumn = true,
                CanResetPasswordRecord = true,
                PopupWidth = 800,
                PopupHeight = 400
            };

            viewModel.GridViewModel = BuildGridViewModel(_gridConfigService, gridViewModel);
            return View(viewModel);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.View)]
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
                    CustomTemplate = "courierTemplate"
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 9,
                    ColumnWidth = 80,
                    Name = "CarNo", 
                    Text = "Car Number",
                    ColumnJustification = GridColumnJustification.Left
                },
                new ViewColumnViewModel
                {
                    ColumnOrder = 10,
                    ColumnWidth = 80,
                    Name = "CleanImei", 
                    Text = "Force Sign Out",
                    ColumnJustification = GridColumnJustification.Left,
                    CustomTemplate = "courierCleanImeiTemplate",
                    Sortable=false
                }

            };
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.User, OperationAction = OperationAction.View)]
        public ActionResult GetCourierInfo(int id)
        {
            var obj = _courierService.GetCourierInfo(id);

            return Json(new { Error = SystemMessageLookup.GetMessage("InvalidData") }, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.View)]
        public JsonResult GetDataForGrid(QueryInfo queryInfo)
        {
            return base.GetDataForGridMasterFile(queryInfo);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.View)]
        public ActionResult GetListCourierForDashboard(QueryInfo queryInfo)
        {

            var querryData = _courierService.GetListCourierForDashboard(queryInfo);

            return Json(querryData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLookup(LookupQuery queryInfo)
        {
            var selector = new Func<Courier, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.User != null ? Framework.Utility.CaculatorHelper.GetFullName(o.User.FirstName, o.User.MiddleName, o.User.LastName) : ""
            });
            var result = base.GetLookupForEntity(queryInfo, selector);

            return result;

        }

        public JsonResult GetLookupForTracking(LookupQuery queryInfo)
        {
            var selector = new Func<Courier, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.User != null ? Framework.Utility.CaculatorHelper.GetFullName(o.User.FirstName, o.User.MiddleName, o.User.LastName) : ""
            });

            var queryData = _courierService.GetLookupForTracking(queryInfo, selector);
            queryData.Insert(0, new LookupItemVo() { DisplayName = "---All---", KeyId = 0 });
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        public JsonResult GetLookupWithHeader(LookupQuery queryInfo)
        {
            var selector = new Func<Courier, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.User != null ? Framework.Utility.CaculatorHelper.GetFullName(o.User.FirstName, o.User.MiddleName, o.User.LastName) : ""
            });
            var result = base.GetLookupForEntity(queryInfo, selector);
            var autoAssignItem = new LookupItemVo
            {
                KeyId = 0,
                DisplayName = "---Select courier---"
            };
            ((List<LookupItemVo>)result.Data).Insert(0, autoAssignItem);

            return result;

        }
        //create
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.Add)]
        public ActionResult Create()
        {
            var viewModel = new DashboardCourierDataViewModel
            {
                SharedViewModel = new DashboardCourierShareViewModel
                {
                    CreateMode = true,
                    UserShareViewModel = new DashboardUserShareViewModel()
                    {
                        IsActive = true
                    }
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.Add)]
        public JsonResult Create(CourierParameter parameters)
        {
            //get franchisee configuration
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var packageInfo = _webApiConsumeUserService.GetListPackageChange(objFranchiseeAndLicense);
            var currentPackage = packageInfo.OrderByDescending(o => o.Id).Where(o => o.IsApply).FirstOrDefault();

            //Lay so luong courier dc cho phep cua franchisee
            int numberAllow = 2;
            if (currentPackage != null)
            {
                numberAllow = CaculatorHelper.GetNumberUserAllow(currentPackage.PackageId);
            }

            var numberOfUserCurrent = _userService.Count(o=>o.UserRoleId == 2);

            if (numberAllow <= numberOfUserCurrent)
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("NumberUserAllow"));
                MasterFileService.ThrowCustomValidation(mess);
            }
            //Bat dau Add
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Courier>();
            entity.Status = (int)StatusCourier.Offline;
            // Generate password
            const string randomPassword = "123456";
            if (entity.User.UserName != null)
            {
                entity.User.Password = PasswordHelper.HashString(randomPassword, entity.User.UserName);
            }

            var sharViewModel = viewModel.SharedViewModel as DashboardCourierShareViewModel;//
            var logoFilePath = "";
            if (sharViewModel != null)
            {
                if (!String.IsNullOrEmpty(sharViewModel.UserShareViewModel.Avatar))
                {
                    if (!sharViewModel.UserShareViewModel.Avatar.Contains("data:image/jpg;base64"))
                    {
                        logoFilePath = Server.MapPath(sharViewModel.UserShareViewModel.Avatar);
                        entity.User.Avatar = _resizeImage.ResizeImageByHeightAndWidth(logoFilePath, 450, 450);
                    }
                }
            }



            var savedEntity = MasterFileService.Add(entity);
            if (savedEntity.Id > 0)
            {
                var webLink = AppSettingsReader.GetValue("Url", typeof(String)) as string;
                var imgSrc = webLink + "/Content/quickspatch/img/logo-o.svg";
                var urlChangePass = webLink + "/Authentication/ChangePasswordCourier?code=" + PasswordHelper.HashString(savedEntity.Id.ToString(), entity.User.UserName);
                var fromEmail = AppSettingsReader.GetValue("EmailFrom", typeof(String)) as string;
                var webapi = AppSettingsReader.GetValue("WebApiUrlFranchisee", typeof(String)) as string;
                var displayName = AppSettingsReader.GetValue("EmailFromDisplayName", typeof(String)) as string;
                //var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                var franchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "";
                var emailContent = Framework.Utility.TemplateHelpper.FormatTemplateWithContentTemplate(
                    TemplateHelpper.ReadContentFromFile(TemplateConfigFile.CreateCourierEmailTemplate, true),
                    new
                    {
                        img_src = imgSrc,
                        full_name = Framework.Utility.CaculatorHelper.GetFullName(savedEntity.User.FirstName, savedEntity.User.MiddleName, savedEntity.User.LastName),
                        web_link = webLink,
                        user_name = entity.User.UserName,
                        password = randomPassword,
                        url_change_pass = urlChangePass,
                        franchisee_Name = franchiseeName,
                        web_api = webapi
                    });
                // send email
                var logo = franchiseeConfiguration != null ? franchiseeConfiguration.Logo : new byte[0];
                _emailHandler.SendEmailSsl(fromEmail, new[] { savedEntity.User.Email }, SystemMessageLookup.GetMessage("SubjectToSendEmailForCreateUser"),
                                        emailContent, logo, true, displayName);
            }
            return
                Json(new
                {
                    id = savedEntity.Id,
                    name =
                        Framework.Utility.CaculatorHelper.GetFullName(savedEntity.User.FirstName,
                            savedEntity.User.MiddleName, savedEntity.User.LastName)
                }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCouriersForSchedule(QueryInfo queryInfo)
        {
            var listCourierForSchedule = _courierService.GetCouriersForSchedule(queryInfo);
            return Json(listCourierForSchedule, JsonRequestBehavior.AllowGet);
        }

        //delete
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {
            if (AuthenticationService.GetCurrentUser().User.Id == id)
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("CannotDeleteYourself"));
                MasterFileService.ThrowCustomValidation(mess);
            }

            var courier = MasterFileService.GetById(id);
            if (courier != null && courier.Requests.Count > 0)
            {
                var mess = string.Format(SystemMessageLookup.GetMessage("DeleteCourierHasRequest"));
                MasterFileService.ThrowCustomValidation(mess);
            }
            MasterFileService.DeleteById(id);
            _userService.DeleteById(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //clean mei
        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.Update)]
        public ActionResult CleanImei(int id)
        {
            var courier = _courierService.FirstOrDefault(o => o.Id == id);
            courier.Imei = string.Empty;
            courier.Status = (int)StatusCourier.Offline;
            _courierService.UpdateWithouCheckBussinessRule(courier);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        //Update

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return View(viewModel);
        }
        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.Update)]
        public ActionResult Update(CourierParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);



            byte[] lastModified = null;
            var logoFilePath = "";

            if (ModelState.IsValid)
            {
                var sharViewModel = viewModel.SharedViewModel as DashboardCourierShareViewModel;
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);

                if (sharViewModel != null)
                {
                    if (!String.IsNullOrEmpty(sharViewModel.UserShareViewModel.Avatar))
                    {
                        if (!sharViewModel.UserShareViewModel.Avatar.Contains("data:image/jpg;base64"))
                        {
                            logoFilePath = Server.MapPath(sharViewModel.UserShareViewModel.Avatar);
                            mappedEntity.User.Avatar = _resizeImage.ResizeImageByHeightAndWidth(logoFilePath, 450, 450);
                        }
                    }
                }

                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.View)]
        public JsonResult ExportExcel(List<ColumnModel> gridColumns, QueryInfo queryInfo)
        {
            return base.ExportExcelMasterfile(gridColumns, queryInfo);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.View)]
        public FileResult DownloadExcelFile(string fileName)
        {
            return base.DownloadExcelMasterFile(fileName);
        }

        //reset password
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.Update)]
        public ActionResult ResetPassword(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return View(viewModel);
        }
        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.Update)]
        public ActionResult ResetPassword(int id, string password, string username)
        {
            var user = _userService.GetById(id);
            if (user != null)
            {
                string randomPassword = password;
                user.Password = PasswordHelper.HashString(randomPassword, user.UserName);
                _userService.Update(user);

                var webLink = AppSettingsReader.GetValue("Url", typeof(String)) as string;
                var imgSrc = webLink + "/Content/quickspatch/img/logo-o.svg";
                var urlChangePass = webLink + "/Authentication/ChangePasswordCourier?code=" + PasswordHelper.HashString(id.ToString(), user.UserName);
                var fromEmail = AppSettingsReader.GetValue("EmailFrom", typeof(String)) as string;
                var webapi = AppSettingsReader.GetValue("WebApiUrlFranchisee", typeof(String)) as string;
                var displayName = AppSettingsReader.GetValue("EmailFromDisplayName", typeof(String)) as string;
                var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                var franchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "";
                var emailContent = TemplateHelpper.FormatTemplateWithContentTemplate(
                    TemplateHelpper.ReadContentFromFile(TemplateConfigFile.RestorePasswordCourierTemplate, true),
                    new
                    {
                        img_src = imgSrc,
                        full_name = Framework.Utility.CaculatorHelper.GetFullName(user.FirstName, user.MiddleName, user.LastName),
                        web_link = webLink,
                        user_name = user.UserName,
                        password = randomPassword,
                        url_change_pass = urlChangePass,
                        franchisee_Name = franchiseeName,
                        web_api = webapi
                    });
                // send email
                var logo = franchiseeConfiguration != null ? franchiseeConfiguration.Logo : new byte[0];
                _emailHandler.SendEmailSsl(fromEmail, new[] { user.Email }, SystemMessageLookup.GetMessage("SubjectToSendEmailForCreateUser"),
                                        emailContent,logo, true, displayName);
            }


            return Json(true, JsonRequestBehavior.AllowGet);

        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.View)]
        public JsonResult GetAutoAssignCourier()
        {
            var courier = _courierService.GetAutoAssignCourier();
            return Json(courier, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Dashboard, OperationAction = OperationAction.View)]
        public JsonResult GetAllCourierOnlineLocation()
        {
            var courier = _courierService.GetAllCourierOnlineLocation();
            return Json(courier, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Dashboard, OperationAction = OperationAction.View)]
        public JsonResult GetPositionCurrentOfCourier(int courierId)
        {
            var queryData = _courierService.GetPositionCurrentOfCourier(courierId);
            if (queryData == null)
                queryData = string.Empty;
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        public JsonResult GetLookupForReport(LookupQuery queryInfo)
        {
            var selector = new Func<Courier, LookupItemVo>(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.User != null ? CaculatorHelper.GetFullName(o.User.FirstName, o.User.MiddleName, o.User.LastName) : ""
            });

            var queryData = _courierService.GetLookupForReport(queryInfo, selector);
            queryData.Insert(0, new LookupItemVo() { DisplayName = "Select All", KeyId = 0 });
            var clientsJson = Json(queryData.OrderBy(o=>o.KeyId), JsonRequestBehavior.AllowGet);
            return clientsJson;
        }


        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.Courier, OperationAction = OperationAction.View)]
        public JsonResult GetDataCourier()
        {
            var data = _courierService.Get(o => o.User.IsActive).Select(o => new LookupItemVo
            {
                KeyId = o.Id,
                DisplayName = o.User.FullName
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}