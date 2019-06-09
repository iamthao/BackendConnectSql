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

namespace QuickspatchWeb.Controllers
{
    public class TableVersionController : Controller
    {
        private readonly ITableVersionService _tableVersionService;

        public TableVersionController(ITableVersionService tableVersionService)
        {
            _tableVersionService = tableVersionService;
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public ActionResult GetVersion(int tableId)
        {
            var tableInfo = (TableInfo) tableId;
            var data = _tableVersionService.FirstOrDefault(o => o.TableId == tableInfo);
            if (data != null)
            {
                return Json(new {TableId = tableId, data.Version}, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeContext(DocumentTypeKey = DocumentTypeKey.None, OperationAction = OperationAction.View)]
        public ActionResult GetVersionRequest()
        {
            var data = _tableVersionService.Get(o => o.TableId == TableInfo.Location || o.TableId == TableInfo.Courier).Select(s=> new { s.TableId, s.Version}).ToList();
            if (data.Count > 0)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            string version = null;
            return Json(new List<dynamic>() { new { TableId = 1, Version = version }, new { TableId = 2, Version = version }, }, JsonRequestBehavior.AllowGet);
        }
    }
}