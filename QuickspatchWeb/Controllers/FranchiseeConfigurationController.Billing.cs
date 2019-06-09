
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Utility;
using Microsoft.Ajax.Utilities;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.FranchiseeConfiguration;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using System.Transactions;
using System.Web;
using System.Web.Helpers;
using Framework.DomainModel.ValueObject;
using Framework.Service.Translation;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Web.WebPages;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Presentation;


namespace QuickspatchWeb.Controllers
{
    public partial class FranchiseeConfigurationController
    {
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public ActionResult Billing()
        {
            return View();
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public ActionResult BillingIndex()
        {
            return View();
        }

        //Get data for biling index
        public JsonResult GetInfoBillingIndex()
        {
            var account = GetDataForAccountBillingIndex();
            var paymentInfo = GetDataForPaymentBillingIndex();
            var model = new GetInfoBillingIndex
            {
                //Account
                IsTrial = account.IsTrial,
                CurrentPlan = account.CurrentPlan,
                AccountName = account.AccountName,
                AccountOwner = account.AccountOwner,
                Url = account.Url,
                IsClosingAccount = account.IsClosingAccount,

                //Info from Api Get Payment Infor
                AccountStatus = account.AccountStatus,
                NextBillingDate = account.NextBillingDate,
                Address = paymentInfo.Address,
                City = paymentInfo.City,
                Zip = paymentInfo.Zip,
                State = paymentInfo.State,
                NamePaymentInfo = paymentInfo.NamePaymentInfo,
                Phone = paymentInfo.Phone
            };
            var clientsJson = Json(model, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        //Get info for biling index
        private AccountBillingIndexViewModel GetDataForAccountBillingIndex()
        {
            var url = ConfigurationManager.AppSettings["Url"];
            var name = "";
            var username = "";
            //get user admin franchisee defaul
            var user = _userService.FirstOrDefault(o => o.UserRoleId == 1);
            if (user != null)
            {
                name = user.FirstName == "N/A" ? "N/A" : user.FirstName + " " + user.LastName + " " + (user.MiddleName == "N/A" ? "" : user.MiddleName);
                username = user.UserName;
            }

            //get franchisee configuration
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var isClosingAccount = false;
            var franchiseeInfo = _webApiUserService.GetInfoFranchisee(objFranchiseeAndLicense);
            if (franchiseeInfo.CloseDate != null)
            {
                if (franchiseeInfo.CloseDate.GetValueOrDefault() <= DateTime.UtcNow)
                {
                    isClosingAccount = true;
                }
            }

            //get current package
            var packageInfo = _webApiUserService.GetListPackageChange(objFranchiseeAndLicense);
            var currentPackage = packageInfo.OrderByDescending(o => o.Id).FirstOrDefault(o => o.IsApply);

            var nextBillingDate = Convert.ToDateTime(franchiseeInfo.EndActiveDate).ToShortDateString();

            var nameNextPakage = "";
            var nextPackage = packageInfo.OrderByDescending(o => o.Id).FirstOrDefault();

            decimal priceNextPackage = 0;
            bool isTrial = false;
            var description = "";
            if (currentPackage != null)
            {
                priceNextPackage = CaculatorHelper.GetPricePackage(currentPackage.PackageId);
                isTrial = currentPackage.PackageId == 0;
                description = CaculatorHelper.GetDescriptionPackage(currentPackage.PackageId);
            }
            if (nextPackage != null)
            {
                if (nextPackage.IsApply == false)
                {
                    nameNextPakage = nextPackage.PackageName;
                    priceNextPackage = CaculatorHelper.GetPricePackage(nextPackage.PackageId);
                }
            }

            var account = new AccountBillingIndexViewModel
            {
                IsTrial = isTrial,
                CurrentPlan = description,
                AccountStatus = " Ends " + nextBillingDate,
                AccountName = username,
                AccountOwner = name,
                Url = url,
                IsClosingAccount = isClosingAccount,
                NextBillingDate = isTrial ? "N/A" : nextBillingDate + " " + nameNextPakage + " (" + priceNextPackage + "$)"
            };
            return account;
        }
        private int GetCurrentRequestId()
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };

            var packageInfo = _webApiUserService.GetListPackageChange(objFranchiseeAndLicense);
            if (packageInfo != null)
                return packageInfo[0].RequestId;
            return 0;
        }
        private PaymentBillingIndexViewModel GetDataForPaymentBillingIndex()
        {
            if (GetCurrentRequestId() == 0)
            {
                var data1 = new PaymentBillingIndexViewModel
                {
                    Address = "N/A",
                    City = "N/A",
                    Zip = "N/A",
                    State = "N/A",
                    NamePaymentInfo = "N/A",
                    Phone = "N/A",
                };
                return data1;
            }

            //get payment info from api payment
            var payment = new PaymentInfoApiDto
            {
                ProductKey = ConfigurationManager.AppSettings["ProductKey"],
                SecretKey = ConfigurationManager.AppSettings["SecretKey"],
                Id = GetCurrentRequestId()
            };
            var paymentInfo = _webApiPaymentService.GetPaymentInfo(payment);

            var firstName = paymentInfo.FirstName ?? "";
            var lastName = paymentInfo.LastName ?? "";
            var middleName = paymentInfo.MiddleName ?? "";

            //function end date from api payment
            //var nextBillingDate = "";
            //if (paymentInfo.SubmitedOn != null)
            //{
            //    var dateCreate = Convert.ToDateTime(paymentInfo.SubmitedOn);
            //    nextBillingDate = CaculatorHelper.GetNextMonth(dateCreate).ToShortDateString();
            //}

            //var endDate = info != null ? String.Format("{0:MM/dd/yyyy}", info.EndActiveDate) : "";
            var data = new PaymentBillingIndexViewModel
            {
                Address = paymentInfo.Address1 ?? "",
                City = paymentInfo.City ?? "",
                Zip = paymentInfo.Zip ?? "",
                State = paymentInfo.State ?? "",
                NamePaymentInfo = CaculatorHelper.GetFullName(firstName, middleName, lastName),
                Phone = paymentInfo.PhoneNumber != null ? paymentInfo.PhoneNumber.ApplyFormatPhone() : "",
            };
            return data;
        }

        //Check neu co package chua Apply
        public JsonResult CheckHasPackageNoApply()
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();

            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var lists = _webApiUserService.GetListPackageChange(objFranchiseeAndLicense);
            var rowFirst = lists.OrderByDescending(o => o.Id).FirstOrDefault();
            if (rowFirst != null)
            {
                if (rowFirst.IsApply != true)
                {
                    var clientsJson = Json(true, JsonRequestBehavior.AllowGet);
                    return clientsJson;
                }
            }
            var clientsJson1 = Json(false, JsonRequestBehavior.AllowGet);
            return clientsJson1;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public ActionResult BillingPackage()
        {
            return View();
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public ActionResult CloseAccount()
        {
            return View();
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.Update)]
        [HttpPost]
        public ActionResult CloseAccount(CloseAccountViewModel item)
        {

            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();

            //var payment = new PaymentInfoApiDto();
            //payment.ProductKey = ConfigurationManager.AppSettings["ProductKey"];
            //payment.SecretKey = ConfigurationManager.AppSettings["SecretKey"];
            //payment.Id = GetCurrentRequestId();
            //var packageInfo = _webApiPaymentService.CancelRequest(payment);
            //if (packageInfo.ResultCode == "OK")
            //{

            if (franchiseeConfiguration != null)
            {
                var data = new FranchiseeTernantCloseAccountDto
                {
                    FranchiseeLicense = franchiseeConfiguration.LicenseKey,
                    FranchiseeName = franchiseeConfiguration.Name,
                    Description = item.Description,
                    Question = item.Question
                };
                var currentUser = AuthenticationService.GetCurrentUser();
                if (currentUser != null)
                {
                    var user = _userService.GetUserByUserNameAndPass(currentUser.User.UserName, item.Password);
                    if (user != null)
                    {
                        return Json(_webApiUserService.UpdateFranchiseeTenantCloseAccount(data));
                    }
                }
            }
            //}

            return Json("errorPassword");
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.Update)]
        [HttpPost]
        public ActionResult CancelAccount()
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            if (franchiseeConfiguration != null)
            {
                var data = new FranchisseNameAndLicenseDto
                {
                    LicenseKey = franchiseeConfiguration.LicenseKey,
                    FranchiseeName = franchiseeConfiguration.Name,
                };
                return Json(_webApiUserService.UpdateFranchiseeTenantCancelAccount(data));
            }
            return Json(false);
        }

        // return page when change package success
        [AllowAnonymous]
        public ActionResult Success()
        {
            return View();
        }

        // return page when change package error
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        //get info current package from Api Admin management
        public JsonResult GetInfoPackageCurrent()
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };

            var packageInfoId = _webApiUserService.GetPackageCurrent(objFranchiseeAndLicense);

            var data = new { PackageId = packageInfoId.PackageId };
            var clientsJson = Json(data, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        //encode anh send data to web payment api
        public JsonResult SubmitChangePackageEncodeData(string packageIdParam, string oldPackageIdParam)
        {
            var packageId = Convert.ToInt32(packageIdParam);
            var oldPackageId = Convert.ToInt32(oldPackageIdParam);
            var isNewRequest = true;
            var isApply = false;

            #region lay config tu webconfig
            var productKey = ConfigurationManager.AppSettings["ProductKey"];
            var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            var url = ConfigurationManager.AppSettings["Url"];
            var isRecurrence = ConfigurationManager.AppSettings["IsRecurrence"];
            //var recurrenceInterval = ConfigurationManager.AppSettings["RecurrenceInterval"];
            var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            var paymentUrl = ConfigurationManager.AppSettings["PaymentUrl"];


            if (string.IsNullOrEmpty(productKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(url) ||
                string.IsNullOrEmpty(isRecurrence) || string.IsNullOrEmpty(passPhrase))
            {
                throw new Exception("Missing configuration ProductKey or SecretKey or Url or IsRecurrence  or PassPhrase");
            }
            #endregion

            var registerPaymentDto = new PaymentInfoDto();
            registerPaymentDto.ProductKey = productKey;
            registerPaymentDto.SecretKey = secretKey;
            registerPaymentDto.ReturnUrl = url + "FranchiseeConfiguration/PaySuccess";
            registerPaymentDto.CancelUrl = url + "/LicenceExtension/Close";
            registerPaymentDto.IsRecurrence = int.Parse(isRecurrence);

            //Tinh  Interval 1 Thang hay 12 Thang
            var recurrenceInterval = 7;
            if (packageId % 2 != 0)
            {
                recurrenceInterval = 7;
            }
            registerPaymentDto.RecurrenceInterval = recurrenceInterval;

            // 1: change package; 2: change paymentInfo
            registerPaymentDto.TransactionType = 1;
            registerPaymentDto.Items = new List<RegisterProduct>
            {
                new RegisterProduct { ItemId = 1, ItemName = CaculatorHelper.GetNamePackage(packageId), ItemQuantity = 1, ItemPrice = CaculatorHelper.GetPricePackage(packageId)}
            };

            //tao chuoi thay doi moi lan submit vi PaymentApi se thong bao loi Duplicate khi request co thong tin giong nhau
            var textdate = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/ | " + DateTime.Now.Hour + "/" + DateTime.Now.Minute + "/" + DateTime.Now.Second;


            #region lay user admin dau tien duoc tao cua franchisee
            var user = _userService.FirstOrDefault(o => o.UserRoleId == 1);

            if (user != null)
            {
                registerPaymentDto.FirstName = user.FirstName;
                registerPaymentDto.MiddleName = user.MiddleName;
                registerPaymentDto.LastName = user.LastName;
                registerPaymentDto.PhoneNumber = user.HomePhone ?? "";
            }
            #endregion

            #region lay thong tin franchisee
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            if (franchiseeConfiguration != null)
            {
                registerPaymentDto.Email = franchiseeConfiguration.PrimaryContactEmail;
                registerPaymentDto.Address1 = franchiseeConfiguration.Address1;
                registerPaymentDto.Address2 = franchiseeConfiguration.Address2;
                registerPaymentDto.Zip = franchiseeConfiguration.Zip;
                registerPaymentDto.City = franchiseeConfiguration.City;
                registerPaymentDto.State = franchiseeConfiguration.State;
            }
            #endregion

            #region lay AccountNumber, RequestId current tu api admin
            var info = _webApiUserService.GetInfoFranchisee(objFranchiseeAndLicense);
            var remainAmountFranchisee = info.Amount.GetValueOrDefault();
            registerPaymentDto.AccountNumber = info.AccountNumber;

            var lists = _webApiUserService.GetListPackageChange(objFranchiseeAndLicense);
            if (lists.Count > 0)
            {
                registerPaymentDto.RequestId = lists[0].RequestId;
            }
            else
            {
                registerPaymentDto.RequestId = 0;
            }

            #endregion

            #region Tinh TrialAmount va ngay bat dau
            #region TH: Trial  -> Cac goi
            if (oldPackageId == 0)
            {
                isApply = true;
                registerPaymentDto.StartDate = DateTime.UtcNow;
                registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId);
                isNewRequest = true;
                registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new
                {
                    PackageId = packageId,
                    OldPackage = oldPackageId,
                    DateTimeSend = textdate,
                    StartDate = registerPaymentDto.StartDate,
                    EndDate = packageId % 2 != 0 ? registerPaymentDto.StartDate.AddMonths(12) : registerPaymentDto.StartDate.AddMonths(1),
                    IsApply = isApply,
                    FranchiseeId = info.Id,
                    AccountName = info.AccountNumber,
                });
            }
            #endregion
            else
            {
                // TH : Thang qua Thang
                if (oldPackageId % 2 == 0 && packageId % 2 == 0)
                {
                    #region Goi moi nho hon goi cu
                    if (oldPackageId > packageId)
                    {
                        registerPaymentDto.StartDate = info.EndActiveDate ?? DateTime.UtcNow;


                        //So tien con lai lon hon so tien tra cho goi ke tiep
                        if (remainAmountFranchisee > CaculatorHelper.GetPricePackage(packageId))
                        {
                            var paymentInfoDto = new PaymentInfoApiDto
                            {
                                Id = registerPaymentDto.RequestId,
                                ProductKey = productKey,
                                SecretKey = secretKey
                            };
                            var cancel = _webApiPaymentService.CancelRequest(paymentInfoDto);

                            var packageHistory = new AddPackageHistoryDto
                            {
                                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                                PackageId = packageId,
                                OldPackageId = oldPackageId,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(1),
                                RequestId = 0,
                                FranchiseeTenantId = info.Id,
                                AccountNumber = info.AccountNumber,
                                IsApply = false,
                                Amount = remainAmountFranchisee,
                                NextBillingDate = registerPaymentDto.StartDate,
                                PackageNextBillingDate = packageId
                            };

                            _webApiUserService.AddPackageHistory(packageHistory);

                            var returnData1 = new { PaymentUrl = paymentUrl, Data = "", IsNewRequest = false };
                            var clientsJson1 = Json(returnData1, JsonRequestBehavior.AllowGet);
                            return clientsJson1;
                        }
                        //So tien con lai nho hon hoac bang so tien tra cho goi ke tiep tao request 
                        else
                        {
                            registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId) - remainAmountFranchisee;
                            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new
                            {
                                PackageId = packageId,
                                OldPackage = oldPackageId,
                                DateTimeSend = textdate,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(1),
                                IsApply = false,
                                FranchiseeId = info.Id,
                                AccountName = info.AccountNumber,
                            });
                        }
                    }
                    #endregion
                    #region Package moi lon hon Package cu , apply lien
                    else
                    {
                        registerPaymentDto.StartDate = DateTime.UtcNow;
                        //Tinh co tien con lai cua Package cu
                        var startDate = info.EndActiveDate.GetValueOrDefault().AddMonths(-1);
                        var usedDay = DateTimeHelper.TotalDaysUsed(startDate, DateTime.UtcNow);//CaculatorHelper.DateDiffAsString(startDate, DateTime.UtcNow, 2); 
                        decimal amountUsedDay = (CaculatorHelper.GetPricePackage(oldPackageId) / 30) * usedDay;
                        var remainAmount = CaculatorHelper.GetPricePackage(oldPackageId) - Math.Round(amountUsedDay, 0, MidpointRounding.AwayFromZero);

                        //So tien con lai lon hon so tien tra cho goi ke tiep
                        if (remainAmountFranchisee + remainAmount > CaculatorHelper.GetPricePackage(packageId))
                        {
                            var paymentInfoDto = new PaymentInfoApiDto
                            {
                                Id = registerPaymentDto.RequestId,
                                ProductKey = productKey,
                                SecretKey = secretKey
                            };
                            //huy request hien tai de ko tru tien vao thang ke tiep
                            var cancel = _webApiPaymentService.CancelRequest(paymentInfoDto);

                            var packageHistory = new AddPackageHistoryDto
                            {
                                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                                PackageId = packageId,
                                OldPackageId = oldPackageId,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(1),
                                RequestId = 0,
                                FranchiseeTenantId = info.Id,
                                AccountNumber = info.AccountNumber,
                                IsApply = true,
                                Amount = remainAmountFranchisee + remainAmount - CaculatorHelper.GetPricePackage(packageId),
                                NextBillingDate = registerPaymentDto.StartDate.AddMonths(1),
                                PackageNextBillingDate = packageId
                            };

                            _webApiUserService.AddPackageHistory(packageHistory);

                            var returnData1 = new { PaymentUrl = paymentUrl, Data = "", IsNewRequest = false };
                            var clientsJson1 = Json(returnData1, JsonRequestBehavior.AllowGet);
                            return clientsJson1;
                        }
                        //So tien con lai nho hon hoac bang so tien tra cho goi ke tiep tao request 
                        else
                        {
                            registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId) - remainAmountFranchisee - remainAmount;
                            registerPaymentDto.StartDate = DateTime.UtcNow;
                            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new
                            {
                                PackageId = packageId,
                                OldPackage = oldPackageId,
                                DateTimeSend = textdate,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(1),
                                IsApply = true,
                                FranchiseeId = info.Id,
                                AccountName = info.AccountNumber,
                            });
                        }
                    }
                    #endregion
                }

                // TH : Nam qua Nam
                if (oldPackageId % 2 != 0 && packageId % 2 != 0)
                {
                    #region Goi moi nho hon goi cu. Cuoi nam apply
                    if (oldPackageId > packageId)
                    {
                        #region So tien con lai lon hon so tien tra cho goi ke tiep
                        if (remainAmountFranchisee  > CaculatorHelper.GetPricePackage(packageId))
                        {
                            var paymentInfoDto = new PaymentInfoApiDto
                            {
                                Id = registerPaymentDto.RequestId,
                                ProductKey = productKey,
                                SecretKey = secretKey
                            };
                            var cancel = _webApiPaymentService.CancelRequest(paymentInfoDto);

                            var packageHistory = new AddPackageHistoryDto
                            {
                                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                                PackageId = packageId,
                                OldPackageId = oldPackageId,
                                StartDate = info.EndActiveDate.GetValueOrDefault(),
                                EndDate = info.EndActiveDate.GetValueOrDefault().AddMonths(12),
                                RequestId = 0,
                                FranchiseeTenantId = info.Id,
                                AccountNumber = info.AccountNumber,
                                IsApply = false,
                                Amount = remainAmountFranchisee,
                                NextBillingDate = info.EndActiveDate.GetValueOrDefault(),
                                PackageNextBillingDate = packageId
                            };

                            _webApiUserService.AddPackageHistory(packageHistory);

                            var returnData1 = new { PaymentUrl = paymentUrl, Data = "", IsNewRequest = false };
                            var clientsJson1 = Json(returnData1, JsonRequestBehavior.AllowGet);
                            return clientsJson1;
                        }
                        #endregion
                        //So tien con lai nho hon hoac bang so tien tra cho goi ke tiep tao request 
                        else
                        {
                            registerPaymentDto.StartDate = info.EndActiveDate.GetValueOrDefault();
                            registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId) - remainAmountFranchisee;
                            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new
                            {
                                PackageId = packageId,
                                OldPackage = oldPackageId,
                                DateTimeSend = textdate,
                                StartDate = info.EndActiveDate.GetValueOrDefault(),
                                EndDate = info.EndActiveDate.GetValueOrDefault().AddMonths(12),
                                IsApply = false,
                                FranchiseeId = info.Id,
                                AccountName = info.AccountNumber,
                            });
                        }
                    }
                    #endregion
                    #region Package moi lon hon Package cu , apply lien
                    else
                    {
                        registerPaymentDto.StartDate = DateTime.UtcNow;
                        //Tinh co tien con lai cua Package cu
                        var remainMonth = 12- DateTimeHelper.TotalMonthsUsed(info.EndActiveDate.GetValueOrDefault().AddMonths(-12),DateTime.UtcNow);//CaculatorHelper.DateDiffAsString(DateTime.UtcNow, info.EndActiveDate.GetValueOrDefault(),1);
                        decimal amountRemainMonth = CaculatorHelper.GetPricePackage(oldPackageId) / 12;
                        var remainAmount = remainMonth * amountRemainMonth;

                        //So tien con lai lon hon so tien tra cho goi ke tiep
                        if (remainAmountFranchisee + remainAmount > CaculatorHelper.GetPricePackage(packageId))
                        {
                            var paymentInfoDto = new PaymentInfoApiDto
                            {
                                Id = registerPaymentDto.RequestId,
                                ProductKey = productKey,
                                SecretKey = secretKey
                            };
                            //huy request hien tai de khong bi tru tien vao chu ky ke tiep
                            var cancel = _webApiPaymentService.CancelRequest(paymentInfoDto);

                            var packageHistory = new AddPackageHistoryDto
                            {
                                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                                PackageId = packageId,
                                OldPackageId = oldPackageId,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(12),
                                RequestId = 0,
                                FranchiseeTenantId = info.Id,
                                AccountNumber = info.AccountNumber,
                                IsApply = true,
                                Amount = remainAmountFranchisee + remainAmount - CaculatorHelper.GetPricePackage(packageId),
                                NextBillingDate = registerPaymentDto.StartDate.AddMonths(12),
                                PackageNextBillingDate = packageId
                            };

                            _webApiUserService.AddPackageHistory(packageHistory);

                            var returnData1 = new { PaymentUrl = paymentUrl, Data = "", IsNewRequest = false };
                            var clientsJson1 = Json(returnData1, JsonRequestBehavior.AllowGet);
                            return clientsJson1;
                        }
                        //So tien con lai nho hon hoac bang so tien tra cho goi ke tiep tao request 
                        else
                        {
                            registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId) - remainAmountFranchisee - remainAmount;
                            registerPaymentDto.StartDate = DateTime.UtcNow;
                            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new
                            {
                                PackageId = packageId,
                                OldPackage = oldPackageId,
                                DateTimeSend = textdate,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(12),
                                IsApply = true,
                                FranchiseeId = info.Id,
                                AccountName = info.AccountNumber,
                            });
                        }
                    }
                    #endregion
                }

                // TH : Thang qua Nam
                if (oldPackageId % 2 == 0 && packageId % 2 != 0)
                {
                    #region Goi moi nho hon goi cu
                    if (oldPackageId > packageId)
                    {
                        registerPaymentDto.StartDate = info.EndActiveDate ?? DateTime.UtcNow;

                        //So tien con lai lon hon so tien tra cho goi ke tiep
                        if (remainAmountFranchisee > CaculatorHelper.GetPricePackage(packageId))
                        {
                            var paymentInfoDto = new PaymentInfoApiDto
                            {
                                Id = registerPaymentDto.RequestId,
                                ProductKey = productKey,
                                SecretKey = secretKey
                            };
                            var cancel = _webApiPaymentService.CancelRequest(paymentInfoDto);

                            var packageHistory = new AddPackageHistoryDto
                            {
                                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                                PackageId = packageId,
                                OldPackageId = oldPackageId,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(12),
                                RequestId = 0,
                                FranchiseeTenantId = info.Id,
                                AccountNumber = info.AccountNumber,
                                IsApply = false,
                                Amount = remainAmountFranchisee,
                                NextBillingDate = registerPaymentDto.StartDate,
                                PackageNextBillingDate = packageId
                            };

                            _webApiUserService.AddPackageHistory(packageHistory);

                            var returnData1 = new { PaymentUrl = paymentUrl, Data = "", IsNewRequest = false };
                            var clientsJson1 = Json(returnData1, JsonRequestBehavior.AllowGet);
                            return clientsJson1;
                        }
                        //So tien con lai nho hon hoac bang so tien tra cho goi ke tiep tao request 
                        else
                        {
                            registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId) - remainAmountFranchisee;
                            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new
                            {
                                PackageId = packageId,
                                OldPackage = oldPackageId,
                                DateTimeSend = textdate,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(12),
                                IsApply = false,
                                FranchiseeId = info.Id,
                                AccountName = info.AccountNumber,
                            });
                        }
                    }
                    #endregion
                    #region Package moi lon hon Package cu , apply lien
                    else
                    {
                        registerPaymentDto.StartDate = DateTime.UtcNow;
                        //Tinh co tien con lai cua Package cu
                        var startDate = info.EndActiveDate.GetValueOrDefault().AddMonths(-1);
                        var usedDay = DateTimeHelper.TotalDaysUsed(startDate, DateTime.UtcNow);//CaculatorHelper.DateDiffAsString(startDate, DateTime.UtcNow, 2);
                        decimal amountUsedDay = (CaculatorHelper.GetPricePackage(oldPackageId) / 30) * usedDay;
                        var remainAmount = CaculatorHelper.GetPricePackage(oldPackageId) - Math.Round(amountUsedDay, 0, MidpointRounding.AwayFromZero);

                        //So tien con lai lon hon so tien tra cho goi ke tiep
                        if (remainAmountFranchisee + remainAmount > CaculatorHelper.GetPricePackage(packageId))
                        {
                            var paymentInfoDto = new PaymentInfoApiDto
                            {
                                Id = registerPaymentDto.RequestId,
                                ProductKey = productKey,
                                SecretKey = secretKey
                            };
                            var cancel = _webApiPaymentService.CancelRequest(paymentInfoDto);

                            var packageHistory = new AddPackageHistoryDto
                            {
                                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                                PackageId = packageId,
                                OldPackageId = oldPackageId,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(12),
                                RequestId = 0,
                                FranchiseeTenantId = info.Id,
                                AccountNumber = info.AccountNumber,
                                IsApply = true,
                                Amount = remainAmountFranchisee + remainAmount - CaculatorHelper.GetPricePackage(packageId),
                                NextBillingDate = registerPaymentDto.StartDate.AddMonths(12),
                                PackageNextBillingDate = packageId
                            };

                            _webApiUserService.AddPackageHistory(packageHistory);

                            var returnData1 = new { PaymentUrl = paymentUrl, Data = "", IsNewRequest = false };
                            var clientsJson1 = Json(returnData1, JsonRequestBehavior.AllowGet);
                            return clientsJson1;
                        }
                        //So tien con lai nho hon hoac bang so tien tra cho goi ke tiep tao request 
                        else
                        {
                            registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId) - remainAmountFranchisee - remainAmount;
                            registerPaymentDto.StartDate = DateTime.UtcNow;
                            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new
                            {
                                PackageId = packageId,
                                OldPackage = oldPackageId,
                                DateTimeSend = textdate,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(12),
                                IsApply = true,
                                FranchiseeId = info.Id,
                                AccountName = info.AccountNumber,
                            });
                        }
                    }
                    #endregion
                }

                // TH : Nam qua Thang
                if (oldPackageId % 2 != 0 && packageId % 2 == 0)
                {
                    #region Goi moi nho hon goi cu
                    if (oldPackageId > packageId || oldPackageId + 1 == packageId)
                    {
                        registerPaymentDto.StartDate = info.EndActiveDate ?? DateTime.UtcNow;

                        //So tien con lai lon hon so tien tra cho goi ke tiep
                        if (remainAmountFranchisee > CaculatorHelper.GetPricePackage(packageId))
                        {
                            var paymentInfoDto = new PaymentInfoApiDto
                            {
                                Id = registerPaymentDto.RequestId,
                                ProductKey = productKey,
                                SecretKey = secretKey
                            };
                            var cancel = _webApiPaymentService.CancelRequest(paymentInfoDto);

                            var packageHistory = new AddPackageHistoryDto
                            {
                                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                                PackageId = packageId,
                                OldPackageId = oldPackageId,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(1),
                                RequestId = 0,
                                FranchiseeTenantId = info.Id,
                                AccountNumber = info.AccountNumber,
                                IsApply = false,
                                Amount = remainAmountFranchisee,
                                NextBillingDate = registerPaymentDto.StartDate,
                                PackageNextBillingDate = packageId
                            };

                            _webApiUserService.AddPackageHistory(packageHistory);

                            var returnData1 = new { PaymentUrl = paymentUrl, Data = "", IsNewRequest = false };
                            var clientsJson1 = Json(returnData1, JsonRequestBehavior.AllowGet);
                            return clientsJson1;
                        }
                        //So tien con lai nho hon hoac bang so tien tra cho goi ke tiep tao request 
                        else
                        {
                            registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId) - remainAmountFranchisee;
                            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new
                            {
                                PackageId = packageId,
                                OldPackage = oldPackageId,
                                DateTimeSend = textdate,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(1),
                                IsApply = false,
                                FranchiseeId = info.Id,
                                AccountName = info.AccountNumber,
                            });
                        }
                    }
                    #endregion
                    #region Package moi lon hon Package cu , apply lien
                    else
                    {
                        registerPaymentDto.StartDate = DateTime.UtcNow;
                        //Tinh co tien con lai cua Package cu
                        var remainMonth = 12 - DateTimeHelper.TotalMonthsUsed(info.EndActiveDate.GetValueOrDefault().AddMonths(-12), DateTime.UtcNow); //CaculatorHelper.DateDiffAsString(DateTime.UtcNow, info.EndActiveDate.GetValueOrDefault(), 1);
                        decimal amountRemainMonth = CaculatorHelper.GetPricePackage(oldPackageId) / 12;
                        var remainAmount = remainMonth * amountRemainMonth;

                        //So tien con lai lon hon so tien tra cho goi ke tiep
                        if (remainAmountFranchisee + remainAmount > CaculatorHelper.GetPricePackage(packageId))
                        {
                            var paymentInfoDto = new PaymentInfoApiDto
                            {
                                Id = registerPaymentDto.RequestId,
                                ProductKey = productKey,
                                SecretKey = secretKey
                            };
                            var cancel = _webApiPaymentService.CancelRequest(paymentInfoDto);

                            var packageHistory = new AddPackageHistoryDto
                            {
                                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                                PackageId = packageId,
                                OldPackageId = oldPackageId,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(1),
                                RequestId = 0,
                                FranchiseeTenantId = info.Id,
                                AccountNumber = info.AccountNumber,
                                IsApply = true,
                                Amount = remainAmountFranchisee + remainAmount - CaculatorHelper.GetPricePackage(packageId),
                                NextBillingDate = registerPaymentDto.StartDate.AddMonths(1),
                                PackageNextBillingDate = packageId
                            };

                            _webApiUserService.AddPackageHistory(packageHistory);

                            var returnData1 = new { PaymentUrl = paymentUrl, Data = "", IsNewRequest = false };
                            var clientsJson1 = Json(returnData1, JsonRequestBehavior.AllowGet);
                            return clientsJson1;
                        }
                        //So tien con lai nho hon hoac bang so tien tra cho goi ke tiep tao request 
                        else
                        {
                            registerPaymentDto.TrialAmount = CaculatorHelper.GetPricePackage(packageId) - remainAmountFranchisee - remainAmount;
                            registerPaymentDto.StartDate = DateTime.UtcNow;
                            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new
                            {
                                PackageId = packageId,
                                OldPackage = oldPackageId,
                                DateTimeSend = textdate,
                                StartDate = registerPaymentDto.StartDate,
                                EndDate = registerPaymentDto.StartDate.AddMonths(1),
                                IsApply = true,
                                FranchiseeId = info.Id,
                                AccountName = info.AccountNumber,
                            });
                        }
                    }
                    #endregion
                }
            }
            #endregion

            var data = JsonConvert.SerializeObject(registerPaymentDto);
            var result = EncryptHelper.Encrypt(data, passPhrase);
            var encodeUrl = HttpUtility.UrlEncode(result);
            var encodeBase64 = EncryptHelper.Base64Encode(encodeUrl);

            var returnData = new { PaymentUrl = paymentUrl, Data = encodeBase64, IsNewRequest = true };
            var clientsJson = Json(returnData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        //Lay thong tin tra ve tu Api Payment
        [HttpGet]
        public ActionResult PaySuccess(string data)
        {
            var url = ConfigurationManager.AppSettings["Url"];
            var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];

            if (string.IsNullOrEmpty(passPhrase))
            {
                throw new Exception("Missing configuration PassPhrase");
            }
            var base64EncodedBytes = Convert.FromBase64String(data);
            var decode = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            decode = HttpUtility.UrlDecode(decode);
            var decrypt = EncryptHelper.Decrypt(decode, passPhrase);
            var model = JsonConvert.DeserializeObject<PaymentViewModel>(decrypt);

            if (ModelState.IsValid)
            {
                //get thong tin package hien tai va thong tin franchisee
                var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
                if (model.RequestId != null)
                {
                    var oldPackage = 0;
                    var startDate = DateTime.UtcNow;
                    var endDate = DateTime.UtcNow;
                    var isApply = false;
                    var franchiseeId = 0;
                    var accountName = "";
                    if (!string.IsNullOrEmpty(model.AdditionInfo))
                    {
                        // additionInfo  chua thong tin package da payment
                        var additionInfo = JsonConvert.DeserializeObject<dynamic>(model.AdditionInfo);
                        if (CaculatorHelper.IsPropertyExist(additionInfo, "PackageId"))
                        {
                            model.PackageId = (int?)additionInfo.PackageId;
                        }
                        if (CaculatorHelper.IsPropertyExist(additionInfo, "OldPackage"))
                        {
                            oldPackage = (int)additionInfo.OldPackage;
                        }
                        if (CaculatorHelper.IsPropertyExist(additionInfo, "StartDate"))
                        {
                            startDate = (DateTime)additionInfo.StartDate;
                        }
                        if (CaculatorHelper.IsPropertyExist(additionInfo, "EndDate"))
                        {
                            endDate = (DateTime)additionInfo.EndDate;
                        }
                        if (CaculatorHelper.IsPropertyExist(additionInfo, "IsApply"))
                        {
                            isApply = (bool)additionInfo.IsApply;
                        }
                        if (CaculatorHelper.IsPropertyExist(additionInfo, "FranchiseeId"))
                        {
                            franchiseeId = (int)additionInfo.FranchiseeId;
                        }
                        if (CaculatorHelper.IsPropertyExist(additionInfo, "AccountName"))
                        {
                            accountName = (string)additionInfo.AccountName;
                        }
                    }

                    var packageHistory = new AddPackageHistoryDto
                    {
                        FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                        LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : "",
                        PackageId = model.PackageId ?? 0,
                        RequestId = model.RequestId ?? 0,
                        StartDate = startDate,
                        EndDate = endDate,
                        OldPackageId = oldPackage,
                        FranchiseeTenantId = franchiseeId,
                        AccountNumber = accountName,
                        IsApply = isApply,
                        PackageNextBillingDate = model.PackageId ?? 0,
                    };
                    _webApiUserService.AddPackageHistory(packageHistory);

                    //return Redirect(url + "#/franchiseeconfiguration?tabIndex=2");
                    return Redirect(url + "/LicenceExtension/Close/1");
                }

                //if (addStatus)
                //{
                //    var oldPackage = CaculatorHelper.GetNamePackage(packageInfo.PackageId);
                //    var newPackage = CaculatorHelper.GetNamePackage(model.PackageId);
                //    var webLink = AppSettingsReader.GetValue("Url", typeof(String)) as string;
                //    var imgSrc = webLink + "/Content/img/logo-o.svg";
                //    var fromEmail = AppSettingsReader.GetValue("EmailFrom", typeof(String)) as string;
                //    var displayName = AppSettingsReader.GetValue("EmailFromDisplayName", typeof(String)) as string;
                //    var emailContent = TemplateHelpper.FormatTemplateWithContentTemplate(
                //        TemplateHelpper.ReadContentFromFile(TemplateConfigFile.EmailChangePackageTemplate,
                //            true),
                //        new
                //        {
                //            img_src = imgSrc,
                //            full_name = model.CompanyName,
                //            web_link = webLink,
                //            old_package = oldPackage,
                //            new_package = newPackage,
                //        });
                //    // send email
                //    _emailHandler.SendEmail(fromEmail, new[] { model.Email },
                //        SystemMessageLookup.GetMessage("SubjectToSendEmailForRegisterConfirm"),
                //        emailContent, true, displayName);
                //}
                return RedirectToAction("Error", "FranchiseeConfiguration");
                //return RedirectToAction("Success", "FranchiseeConfiguration");
            }

            return RedirectToAction("Error", "FranchiseeConfiguration");
        }

        //send thong tin nhu change package nhung TransactionType = 2
        public JsonResult ChangePaymentInfoApi()
        {
            var productKey = ConfigurationManager.AppSettings["ProductKey"];
            var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            var url = ConfigurationManager.AppSettings["Url"];
            var isRecurrence = ConfigurationManager.AppSettings["IsRecurrence"];
            var recurrenceInterval = ConfigurationManager.AppSettings["RecurrenceInterval"];
            var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];

            var paymentUrl = ConfigurationManager.AppSettings["PaymentUrl"];


            if (string.IsNullOrEmpty(productKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(url) ||
                string.IsNullOrEmpty(isRecurrence) || string.IsNullOrEmpty(recurrenceInterval) || string.IsNullOrEmpty(passPhrase))
            {
                throw new Exception("Missing configuration ProductKey or SecretKey or Url or IsRecurrence or RecurrenceInterval or PassPhrase");
            }

            var registerPaymentDto = new PaymentInfoDto();
            registerPaymentDto.ProductKey = productKey;
            registerPaymentDto.SecretKey = secretKey;
            //registerPaymentDto.ReturnUrl = url + "#/franchiseeconfiguration?tabIndex=2";
            registerPaymentDto.ReturnUrl = url + "/LicenceExtension/Close";
            registerPaymentDto.CancelUrl = url + "/LicenceExtension/Close";
            registerPaymentDto.IsRecurrence = int.Parse(isRecurrence);
            registerPaymentDto.RecurrenceInterval = int.Parse(recurrenceInterval);
            //0: new; 1: change package; 2: change paymentInfo
            registerPaymentDto.TransactionType = 2;

            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var packageCurrentId = _webApiUserService.GetPackageCurrent(objFranchiseeAndLicense);
            registerPaymentDto.Items = new List<RegisterProduct>
            {
                new RegisterProduct { ItemId = 1, ItemName = CaculatorHelper.GetNamePackage(packageCurrentId.PackageId), ItemQuantity = 1, ItemPrice = CaculatorHelper.GetPricePackage( packageCurrentId.PackageId)}
            };

            var textdate = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/ | " + DateTime.Now.Hour + "/" + DateTime.Now.Minute + "/" + DateTime.Now.Second;
            registerPaymentDto.AdditionInfo = JsonConvert.SerializeObject(new { PackageId = packageCurrentId.PackageId, DateTimeSend = textdate });

            var user = _userService.FirstOrDefault(o => o.UserRoleId == 1);

            if (user != null)
            {
                registerPaymentDto.FirstName = user.FirstName;
                registerPaymentDto.MiddleName = user.MiddleName;
                registerPaymentDto.LastName = user.LastName;
            }

            if (franchiseeConfiguration != null)
            {
                registerPaymentDto.Email = franchiseeConfiguration.PrimaryContactEmail;
                registerPaymentDto.Address1 = franchiseeConfiguration.Address1;
                registerPaymentDto.Address2 = franchiseeConfiguration.Address2;
                registerPaymentDto.Zip = franchiseeConfiguration.Zip;
                registerPaymentDto.City = franchiseeConfiguration.City;
                registerPaymentDto.State = franchiseeConfiguration.State;
            }

            var info = _webApiUserService.GetInfoFranchisee(objFranchiseeAndLicense);
            registerPaymentDto.AccountNumber = info.AccountNumber;

            var lists = _webApiUserService.GetListPackageChange(objFranchiseeAndLicense);
            registerPaymentDto.RequestId = lists.Count > 0 ? lists[0].RequestId : 0;

            var data = JsonConvert.SerializeObject(registerPaymentDto);
            var result = EncryptHelper.Encrypt(data, passPhrase);
            var encodeUrl = HttpUtility.UrlEncode(result);
            var encodeBase64 = EncryptHelper.Base64Encode(encodeUrl);

            var returnData = new { PaymentUrl = paymentUrl, Data = encodeBase64 };
            var clientsJson = Json(returnData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public ActionResult TransactionDetail(string id)
        {
            var transactionDto = new TransactionDetailDto
            {
                TransactionId = id
            };
            var transactionInfo = _webApiPaymentService.GetTransactionDetail(transactionDto);
            var transaction = new TransactionDetailViewModel
            {
                TransactionId = transactionInfo.TransactionId,
                TransactionType = transactionInfo.TransactionType,
                SettleAmount = transactionInfo.SettleAmount + " $",
                Status = transactionInfo.StatusMessage,
                Name = transactionInfo.BillingAddress.firstName + " " + transactionInfo.BillingAddress.lastName,
                PhoneNumber = transactionInfo.BillingAddress.phoneNumber,
                Email = transactionInfo.BillingAddress.email,
                Address = transactionInfo.BillingAddress.address,
                City = transactionInfo.BillingAddress.city,
                State = transactionInfo.BillingAddress.state,
                Zip = transactionInfo.BillingAddress.zip
            };

            return View(transaction);
        }

        //Get data for biling index
        public JsonResult GetInfoCloseAccount()
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };

            var franchiseeInfo = _webApiUserService.GetInfoFranchisee(objFranchiseeAndLicense);

            var clientsJson1 = Json(new
            {
                EndDate = Convert.ToDateTime(franchiseeInfo.EndActiveDate).ToShortDateString(),
                DateChangeMind = Convert.ToDateTime(franchiseeInfo.EndActiveDate).AddDays(-1).ToShortDateString()
            }, JsonRequestBehavior.AllowGet);
            return clientsJson1;
        }

        //kiem tra Transaction cua thang hien tai da co hay chua?
        public JsonResult CheckTransactionExist()
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };

            var franchiseeInfo = _webApiUserService.GetInfoFranchisee(objFranchiseeAndLicense);
            var packageInfoId = _webApiUserService.GetPackageCurrent(objFranchiseeAndLicense);

            if (packageInfoId.RequestId == 0)
            {
                var clientsJson2 = Json(true, JsonRequestBehavior.AllowGet);
                return clientsJson2;
            }
            var listRequestId = new List<int> { packageInfoId.RequestId };

            var fromDate = franchiseeInfo.EndActiveDate.GetValueOrDefault().AddMonths(-1);

            var toDate = franchiseeInfo.EndActiveDate;
            if (packageInfoId.PackageId % 2 == 1)
            {
                fromDate = franchiseeInfo.EndActiveDate.GetValueOrDefault().AddMonths(-12);
            }

            var transaction = new TransactionDto
            {
                FromDate = fromDate,
                ToDate = toDate,
                RequestId = listRequestId
            };

            var getTransaction = _webApiPaymentService.GetListTransaction(transaction);
            if (getTransaction.PaymentTransactionItems.Count > 0)
            {
                var clientsJson = Json(true, JsonRequestBehavior.AllowGet);
                return clientsJson;
            }

            var clientsJson1 = Json(false, JsonRequestBehavior.AllowGet);
            return clientsJson1;
        }

        //check package se dc apply ke tiep = package submit, neu co package chua apply 
        public JsonResult CheckPackageNextApply(string packageId)
        {
            //lay thong tin franchisee
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };

            var franchisee = _webApiUserService.GetInfoFranchisee(objFranchiseeAndLicense);

            var check = true;
            if (franchisee.PackageNextBillingDate != null)
            {
                if (franchisee.PackageNextBillingDate == Convert.ToInt32(packageId))
                {
                    check = false;
                }
            }

            var clientsJson1 = Json(check, JsonRequestBehavior.AllowGet);
            return clientsJson1;
        }

        //check current RequestID da huy chua, neu chua cho ReOpen Account , roi thi ko cho ReOpen
        public JsonResult CheckCurrentRequestCancel()
        {
            //get payment info from api payment
            var payment = new PaymentInfoApiDto
            {
                ProductKey = ConfigurationManager.AppSettings["ProductKey"],
                SecretKey = ConfigurationManager.AppSettings["SecretKey"],
                Id = GetCurrentRequestId()
            };
            var paymentInfo = _webApiPaymentService.GetSubscriptionPaymentStatus(payment);
            if (paymentInfo.StatusCode == "CANCELED")
            {
                var clientsJson = Json(true, JsonRequestBehavior.AllowGet);
                return clientsJson;
            }

            var clientsJson1 = Json(false, JsonRequestBehavior.AllowGet);
            return clientsJson1;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None)]
        public ActionResult ContactUs()
        {
            return PartialView("ContactUs");
        }

        public JsonResult SendContactUs(string fullname, string email, string subject, string content)
        {
            //send email
            var clientsJson = Json(true, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

    }

}