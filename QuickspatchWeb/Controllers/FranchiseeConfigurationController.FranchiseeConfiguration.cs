
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
        public ActionResult FranchiseeConfiguration()
        {
            return View();
        }

        //get data for franchisee configuration
        public JsonResult GetInfoFranchiseeIndex()
        {
            var model = new FranchiseeInfoShareViewModel();
            var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();

            if (franchiseeConfiguration != null)
            {
                var objFranchiseeAndLicense = new FranchisseNameAndLicenseDto
                {
                    FranchiseeName = franchiseeConfiguration.Name,
                    LicenseKey = franchiseeConfiguration.LicenseKey
                };

                var startDate = "";
                var endDate = "";
                var activeDate = _webApiUserService.GetActiveDateLicenseKey(objFranchiseeAndLicense);

                if (activeDate.StartActiveDate != null)
                {
                    startDate = ((DateTime)activeDate.StartActiveDate).ToClientTime("MM/dd/yyyy");
                }
                if (activeDate.EndActiveDate != null)
                {
                    endDate = ((DateTime)activeDate.EndActiveDate).ToClientTime("MM/dd/yyyy");
                }
                model = new FranchiseeInfoShareViewModel
                {
                    FranchiseeId = franchiseeConfiguration.Id.ToString(),
                    Logo = franchiseeConfiguration.Logo != null ? "data:image/jpg;base64," + Convert.ToBase64String(franchiseeConfiguration.Logo) : "",
                    Name = franchiseeConfiguration.Name,
                    OfficePhone = franchiseeConfiguration.OfficePhone.ApplyFormatPhone(),
                    FaxNumber = franchiseeConfiguration.FaxNumber.ApplyFormatPhone(),
                    Address = franchiseeConfiguration.Address1 == "N/A" ? "N/A" : (franchiseeConfiguration.Address1
                                + (franchiseeConfiguration.Address2 == null ? "" : ", " + franchiseeConfiguration.Address2)
                                + ", " + franchiseeConfiguration.City
                                + ", " + franchiseeConfiguration.State
                                + ", " + franchiseeConfiguration.Zip),

                    FranchiseeContact = franchiseeConfiguration.FranchiseeContact,
                    PrimaryContactPhone = franchiseeConfiguration.PrimaryContactPhone.ApplyFormatPhone(),
                    PrimaryContactEmail = franchiseeConfiguration.PrimaryContactEmail,
                    PrimaryContactFax = franchiseeConfiguration.FaxNumber.ApplyFormatPhone(),
                    PrimaryContactCellNumber = franchiseeConfiguration.PrimaryContactCellNumber.ApplyFormatPhone(),

                    StartActiveDate = startDate,
                    EndActiveDate = endDate,
                    LicenseKey = franchiseeConfiguration.LicenseKey,
                };

                var clientsJson = Json(model, JsonRequestBehavior.AllowGet);
                return clientsJson;
            }

            var clientsJson1 = Json(model, JsonRequestBehavior.AllowGet);
            return clientsJson1;
        }

        //get listpackage change
        public JsonResult GetListContact(QueryInfo queryInfo)
        {
            var listContact = _contactService.GetListContact();

            var data = new { Data = listContact, TotalRowCount = listContact.Count };
            var clientsJson = Json(data, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        //set location default
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.Update)]
        public ActionResult UpdateLocationDefault()
        {
            return PartialView("_SharedLocationDefault");
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.None)]
        public JsonResult GetLocationDefault()
        {
            //var franchiseeConfiguration = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            var from =
                _systemConfigurationService.FirstOrDefault(
                    o => o.SystemConfigType == SystemConfigType.DefaultLocationFrom);

            var to =
                _systemConfigurationService.FirstOrDefault(
                    o => o.SystemConfigType == SystemConfigType.DefaultLocationTo);

            var fromId = 0;
            var toId = 0;
            var fromAddress = "";
            var toAddress = "";
            var fromName = "N/A";
            var toName = "N/A";

            if (from != null)
            {
                var frId = Convert.ToInt32(from.Value);
                if (frId > 0)
                {
                    var locationFrom = _locationService.GetById(frId);
                    if (locationFrom != null)
                    {
                        fromId = frId;
                        fromName = locationFrom.Name;
                        fromAddress = CaculatorHelper.GetFullAddressCountry(locationFrom.Address1, locationFrom.Address2, locationFrom.City,
                                        locationFrom.StateOrProvinceOrRegion, locationFrom.Zip, locationFrom.CountryOrRegion.Name);
                    }
                    else
                    {
                        fromId = 0;
                    }

                }               
            }

            if (to != null)
            {
                var tId = Convert.ToInt32(to.Value);
                if (tId > 0)
                {
                    var locationFrom = _locationService.GetById(tId);
                    if (locationFrom != null)
                    {
                        toId = tId;
                        fromName = locationFrom.Name;
                        fromAddress = CaculatorHelper.GetFullAddressCountry(locationFrom.Address1, locationFrom.Address2, locationFrom.City,
                                        locationFrom.StateOrProvinceOrRegion, locationFrom.Zip, locationFrom.CountryOrRegion.Name);
                    }
                    else
                    {
                        toId = 0;
                    }

                }
            }

            var clientsJson = Json(new
            {
                LocationFromId = fromId,
                LocationToId = toId,
                LocationFromAddress = fromAddress,
                LocationToAddress = toAddress,
                LocationFromName = fromName,
                LocationToName = toName,
            }, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.None)]
        public JsonResult UpdateDataLocationDefault(string fromId, string toId)
        {
            var locationFromId = Convert.ToInt32(fromId);
            var locationToId = Convert.ToInt32(toId);

            var franchisee = _franchiseeConfigurationService.GetFranchiseeConfiguration();
            if (franchisee != null)
            {
                if (string.IsNullOrEmpty(franchisee.Address1))
                {
                    franchisee.Address1 = "N/A";
                }
                if (string.IsNullOrEmpty(franchisee.Zip))
                {
                    franchisee.Zip = "N/A";
                }
                if (string.IsNullOrEmpty(franchisee.City))
                {
                    franchisee.City = "N/A";
                }
                if (string.IsNullOrEmpty(franchisee.State))
                {
                    franchisee.State = "N/A";
                }
                franchisee.LocationFromId = locationFromId;
                franchisee.LocationToId = locationToId;
                _franchiseeConfigurationService.Update(franchisee);
                var clientsJson = Json(true, JsonRequestBehavior.AllowGet);
                return clientsJson;
            }
            var clientsJson1 = Json(false, JsonRequestBehavior.AllowGet);
            return clientsJson1;
        }
    }

}