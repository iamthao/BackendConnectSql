
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
        public ActionResult BillingHistory()
        {
            return View();
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public ActionResult HistoryPackage()
        {
            return View();
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public ActionResult HistoryTransaction()
        {
            return View();
        }

        //get listpackage change history
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public JsonResult GetListChangePackage(QueryInfo queryInfo)
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();

            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var lists = _webApiUserService.GetListPackageChange(objFranchiseeAndLicense);

            if (queryInfo.SearchString == null)
            {
                queryInfo.SearchString = "";
            }
            var listSearch =
                lists.Where(
                    o => o.OldPackageName.ToLower().Contains(queryInfo.SearchString) || o.PackageName.ToLower().Contains(queryInfo.SearchString)).ToList();
            var data = new { Data = listSearch, TotalRowCount = listSearch.Count };
            var clientsJson = Json(data, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        //get list transaction  history
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.View)]
        public JsonResult GetListTransaction(TransactionQueryInfo queryInfo)
        {
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();

            var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
            {
                FranchiseeName = franchiseeConfiguration != null ? franchiseeConfiguration.Name : "",
                LicenseKey = franchiseeConfiguration != null ? franchiseeConfiguration.LicenseKey : ""
            };
            var listHistoryPackages = _webApiUserService.GetListPackageChange(objFranchiseeAndLicense);

            var listTransaction = new List<PaymentTransactionItemViewModel>();
            var listRequestId = new List<int>();// { 1075,1078 };
            foreach (var item in listHistoryPackages)
            {
                listRequestId.Add(item.RequestId);
            }
            var transaction = new TransactionDto
            {
                FromDate = DateTime.Now.AddMonths(-1),
                ToDate = DateTime.Now,
                RequestId = listRequestId
            };

            var getTransaction = _webApiPaymentService.GetListTransaction(transaction);
            foreach (var item in getTransaction.PaymentTransactionItems)
            {
                listTransaction.Add(new PaymentTransactionItemViewModel
                {
                    TransactionId = item.TransactionId,
                    SubmittedOnUtc = item.SubmittedOnUtc,
                    RequestId = item.RequestId
                });

            }


            if (queryInfo.SearchString == null)
            {
                queryInfo.SearchString = "";
            }
            if (queryInfo.RequestId > 0)
            {
                var listSearch = listTransaction.Where(o => o.RequestId == queryInfo.RequestId).OrderByDescending(b => b.SubmittedOnUtc).ToList();
                var data = new { Data = listSearch, TotalRowCount = listSearch.Count };
                var clientsJson = Json(data, JsonRequestBehavior.AllowGet);
                return clientsJson;
            }
            else
            {
                var data = new { Data = listTransaction.OrderByDescending(b => b.SubmittedOnUtc).ToList(), TotalRowCount = listTransaction.Count };
                var clientsJson = Json(data, JsonRequestBehavior.AllowGet);
                return clientsJson;
            }
        }
    }

}