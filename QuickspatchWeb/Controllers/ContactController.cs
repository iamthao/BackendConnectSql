using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Entities.Security;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using QuickspatchWeb.Attributes;
using QuickspatchWeb.Models;
using QuickspatchWeb.Models.State;
using Newtonsoft.Json;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;
using Framework.DomainModel.ValueObject;
using QuickspatchWeb.Models.Contact;

namespace QuickspatchWeb.Controllers
{
    public class ContactController : ApplicationControllerGeneric<Contact, DashboardContactDataViewModel>
    {
        private readonly IContactService _contactService;

        public ContactController(IAuthenticationService authenticationService, IDiagnosticService diagnosticService,
            IContactService contactService)
            : base(authenticationService, diagnosticService, contactService)
        {
            _contactService = contactService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.Add)]
        public ActionResult Create()
        {
            var viewModel = new DashboardContactDataViewModel
            {
                SharedViewModel = new DashboardContactShareViewModel()
                {
                    CreateMode = true
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.Add)]
        public int Create(ContactParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<Contact>();
            
            var savedEntity = MasterFileService.Add(entity);
           
            return savedEntity.Id;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.Update)]
        public ActionResult Update(int id)
        {
            var viewModel = GetMasterFileViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.Update)]
        public ActionResult Update(ContactParameter parameters)
        {
            var viewModel = MapFromClientParameters(parameters);

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {

                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);

                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.FranchiseeConfiguration, OperationAction = OperationAction.Delete)]
        public JsonResult Delete(int id)
        {         
            MasterFileService.DeleteById(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}