using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ClosedXML.Excel;
using Framework.DomainModel;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Newtonsoft.Json;
using QuickspatchWeb.Models;
using Framework.Exceptions;
using Framework.Exceptions.DataAccess.Sql;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using Framework.Web.Serializer;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Controllers
{
    /// <summary>
    /// This is the base controller of system
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>

    [OutputCache(Duration = 0, VaryByParam = "*")]
    public abstract class ApplicationControllerGeneric<TEntity, TViewModel> : Controller
        where TEntity : Entity
        where TViewModel : MasterfileViewModelBase<TEntity>, new()
    {
        //protected readonly IAuthenticationService _authenticationService;
        private readonly IDiagnosticService _diagnosticService;
        private readonly IMasterFileService<TEntity> _masterfileService;
        private readonly IAuthenticationService _authenticationService;

        protected readonly AppSettingsReader AppSettingsReader = new AppSettingsReader();
        protected ApplicationControllerGeneric(IAuthenticationService authenticationService, IDiagnosticService diagnosticService,
                                         IMasterFileService<TEntity> masterfileService
            )
        {
            _authenticationService = authenticationService;
            _diagnosticService = diagnosticService;
            _masterfileService = masterfileService;
            // This flag to diable the validate for special characters in the texboxes.
            ValidateRequest = false;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Request.Cookies.AllKeys.Contains("timezoneoffset") && HttpContext.Request.Cookies["timezoneoffset"] != null)
            {
                Session["timezoneoffset"] = HttpContext.Request.Cookies["timezoneoffset"].Value;
            }
            base.OnActionExecuting(filterContext);
        }

        protected IMasterFileService<TEntity> MasterFileService
        {
            get { return _masterfileService; }
        }

        

        protected IAuthenticationService AuthenticationService
        {
            get { return _authenticationService; }
        }

        protected IDiagnosticService DiagnosticService
        {
            get { return _diagnosticService; }
        }

        /// <summary>
        /// Get data for lookup
        /// </summary>
        /// <param name="query"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual JsonResult GetLookupForEntity(LookupQuery query, Func<TEntity, LookupItemVo> selector)
        {
            var queryData = _masterfileService.GetLookup(query, selector);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);
            return clientsJson;
        }

        public virtual JsonResult GetLookupItemForEntity(LookupItem lookupItem, Func<TEntity, LookupItemVo> selector)
        {
            var data = _masterfileService.GetLookupItem(lookupItem, selector);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Build grid
        /// </summary>
        /// <param name="gridConfigService"></param>
        /// <param name="initGridViewModel"></param>
        /// <returns></returns>
        public virtual GridViewModel BuildGridViewModel(IGridConfigService gridConfigService, Func<GridViewModel> initGridViewModel = null)
        {
            var modelName = typeof(TEntity).Name;
            //my be this is a value object, not view model
            var gridViewModel = initGridViewModel != null ? initGridViewModel() : new GridViewModel
            {
                GridId = string.Format("{0}Grid", modelName),
                ModelName = modelName,
                //AdvancedSearchUrl = "~/Views/Shared/AdvancedSearch.cshtml"
            };
            Func<GridConfig, GridConfigViewModel> selector = g => g.MapTo<GridConfigViewModel>();

            var objGridConfig = gridConfigService.GetGridConfig(selector,
                AuthenticationService.GetCurrentUser().User.Id,
                gridViewModel.DocumentTypeId,
                gridViewModel.GridInternalName);
            var objListColumnInGridConfig = new List<ViewColumnViewModel>();
            if (objGridConfig != null && objGridConfig.ViewColumns != null && objGridConfig.ViewColumns.Count != 0)
            {
                gridViewModel.Id = objGridConfig.Id;
                objListColumnInGridConfig = objGridConfig.ViewColumns.OrderBy(o => o.ColumnOrder).ToList();
            }
            var defaultColumns = GetViewColumns();
            foreach (var column in defaultColumns)
            {
                var configColumn = objListColumnInGridConfig.FirstOrDefault(o => o.Name == column.Name);
                if (configColumn != null)
                {
                    column.HideColumn = configColumn.HideColumn;
                    column.ColumnOrder = configColumn.ColumnOrder;
                    column.ColumnWidth = configColumn.ColumnWidth;
                }
            }
            gridViewModel.ViewColumns = defaultColumns.OrderBy(o => o.ColumnOrder).ToList();
            return gridViewModel;
        }

        /// <summary>
        /// Create list column in the grid
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ViewColumnViewModel> GetViewColumns()
        {
            return null;
        }

        protected virtual TViewModel MapFromClientParameters(MasterfileParameter parameters, Action<TViewModel> advanceMapping = null)
        {
            return MapFromClientParameters<TViewModel>(parameters, advanceMapping);
        }

        protected virtual TVModel MapFromClientParameters<TVModel>(MasterfileParameter parameters, Action<TVModel> advanceMapping = null)
            where TVModel : MasterfileViewModelBase<TEntity>, new()
        {
            var viewModel = new TVModel();

            viewModel.ProcessFromClientParameters(parameters);
            if (advanceMapping != null)
            {
                advanceMapping(viewModel);
            }

            return viewModel;
        }

        public virtual TViewModel GetMasterFileViewModel(int id)
        {
            var entity = MasterFileService.GetById(id);
            var viewModel = entity.MapTo<TViewModel>();
            return viewModel;
        }

        public virtual TEntity CreateMasterFile(MasterfileParameter parameters, Action<TViewModel> advanceMapping = null)
        {
            var viewModel = MapFromClientParameters(parameters);
            var entity = viewModel.MapTo<TEntity>();
            var savedEntity = MasterFileService.Add(entity);

            return savedEntity;
        }

        [ChildActionOnly]
        public virtual JsonResult DeleteMultiMasterfile(string selectedRowIdArray, string isDeleteAll)
        {
            if (isDeleteAll == "1")
            {
                MasterFileService.DeleteAll(o => o.Id > 0);
            }
            else
            {
                var liststrId = selectedRowIdArray.Split(',');
                var listId = new List<int>();
                foreach (var item in liststrId)
                {
                    int id;
                    int.TryParse(item, out id);
                    if (id != 0)
                    {
                        listId.Add(id);
                    }
                }
                MasterFileService.DeleteAll(o => listId.Contains(o.Id));
            }
            return Json(new { Error = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetDataForGridMasterFile(QueryInfo queryInfo)
        {
            var queryData = MasterFileService.GetDataForGridMasterfile(queryInfo);
            var clientsJson = Json(queryData, JsonRequestBehavior.AllowGet);

            return clientsJson;
        }

        [ChildActionOnly]
        public virtual JsonResult ExportExcelMasterfile(List<ColumnModel> gridConfig, QueryInfo queryInfo)
        {
            var data = new ExportExcel();
            var tempPath = Path.GetTempPath();
            var dataBind = MasterFileService.GetDataForGridMasterfile(queryInfo);

            string jsonTemp = JsonConvert.SerializeObject(dataBind);
            var dynamicTemp = JsonConvert.DeserializeObject<dynamic>(jsonTemp);
            string dataTemp = JsonConvert.SerializeObject(dynamicTemp.Data);
            var dataItem = JsonConvert.DeserializeObject<List<dynamic>>(dataTemp);

            data.GridConfigViewModel = gridConfig;
            data.ListDataSource = dataItem;

            using (var wb = new XLWorkbook())
            {
                string guid = Guid.NewGuid().ToString();
                string filePath = Path.Combine(tempPath, guid + ".xlsx");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                wb.Worksheets.Add(GenDataTableFromExportExcelType(data), typeof(TEntity).Name).ColumnsUsed().AdjustToContents();
                wb.SaveAs(filePath);
                return Json(new { FileNameResult = guid + ".xlsx", Error = string.Empty }, JsonRequestBehavior.AllowGet);
            }

            //var content = RenderRazorViewToString("~/Views/Shared/Export/_BusinessReportExportContent.cshtml", data);
            //return Json(new { Item = content, Error = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public virtual FileResult DownloadExcelMasterFile(string fileName)
        {
            var tempPath = Path.GetTempPath();
            string filePath = Path.Combine(tempPath, fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, typeof(TEntity).Name + ".xlsx");
        }

        public string RenderRazorViewToString<T>(string viewPath, T model)
        {
            ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewPath);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer);
                viewResult.View.Render(viewContext, writer);
                return writer.GetStringBuilder().ToString();
            }
        }

        protected DataTable GenDataTableFromExportExcelType(ExportExcel data)
        {
            var dt = new DataTable();

            var listColumn = new List<string>();
            foreach (var column in data.GridConfigViewModel)
            {
                if (!string.IsNullOrEmpty(column.Field.Trim()) && !string.IsNullOrEmpty(column.Title.Trim()))
                {
                    listColumn.Add(column.Field);
                }
                dt.Columns.Add(column.Title);
            }

            foreach (var item in data.ListDataSource)
            {
                var array = new object[listColumn.Count];
                for (int i = 0; i < listColumn.Count; i++)
                {
                    array[i] = item[listColumn[i]];
                }
                dt.Rows.Add(array);
            }
            return dt;
        }

        [ChildActionOnly]
        public virtual JsonResult UpdateMasterFile(MasterfileParameter parameters, Action<TViewModel> advanceMapping = null)
        {
            var viewModel = MapFromClientParameters(parameters);

            if (advanceMapping != null)
                advanceMapping.Invoke(viewModel);

            byte[] lastModified = null;

            if (ModelState.IsValid)
            {
                var entity = MasterFileService.GetById(viewModel.SharedViewModel.Id);
                var mappedEntity = viewModel.MapPropertiesToInstance(entity);
                lastModified = MasterFileService.Update(mappedEntity).LastModified;
            }

            return Json(new { Error = string.Empty, Data = new { LastModified = lastModified } }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public virtual JsonResult DeleteMasterFile(TViewModel viewModel)
        {
            var entity = MasterFileService.GetById(viewModel.Id);
            entity.LastModified = viewModel.LastModified;
            MasterFileService.Delete(entity);

            return Json(new { Error = string.Empty }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///     Override default function of controller. this function will use json.net
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data,
                                           string contentType,
                                           Encoding contentEncoding,
                                           JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
        [ChildActionOnly]
        protected dynamic HandleAjaxRequestException(Exception ex)
        {
            //get current error from view model state
            var errors = ViewData
                .ModelState
                .Values
                .SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage)
                .Distinct();

            var feedbackViewModel = BuildFeedBackViewModel(ex, errors);
            return feedbackViewModel;
        }
        [ChildActionOnly]
        protected FeedbackViewModel BuildFeedBackViewModel(Exception ex, IEnumerable<string> modelStateErrors)
        {
            var feedback = new FeedbackViewModel();
            ExceptionHandlingResult exceptionHandlingResult;

            var shouldRethrow = HandleException(ex, out exceptionHandlingResult);

            feedback.Status = shouldRethrow ? FeedbackStatus.Critical : FeedbackStatus.Error;

            feedback.Error = exceptionHandlingResult.ErrorMessage;
            feedback.AddModelStateErrors(modelStateErrors);

            //add more exception from exception stack trace
            feedback.AddModelStateErrors(exceptionHandlingResult.ModelStateErrors);

            return feedback;
        }
        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exceptionHandlingResult"></param>
        /// <returns></returns>
        [ChildActionOnly]
        protected bool HandleException(Exception ex, out ExceptionHandlingResult exceptionHandlingResult)
        {
            exceptionHandlingResult = new ExceptionHandlingResult();
            bool shouldRethrow;

            _diagnosticService.Error(ex);
            _diagnosticService.Error(ex.StackTrace);
            var isProductionMode = IsProductionMode;

            var commonErrorMessage = SystemMessageLookup.GetMessage("GeneralExceptionMessageText");

            //  if production mode then show generic error
            if (isProductionMode)
            {
                exceptionHandlingResult.ErrorMessage = commonErrorMessage;
                exceptionHandlingResult.StackTrace = string.Empty;
            }
            else //  else: show all exception
            {
                exceptionHandlingResult.ErrorMessage = ex.Message;
                exceptionHandlingResult.StackTrace = ex.StackTrace;
            }
            var innerError = ex.InnerException;
            if (innerError != null && !isProductionMode)
            {
                // Check if the error message not is the message from error in the entity framework( this is the error that we can handle, not show to user)
                if (innerError.Message != "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                    exceptionHandlingResult.AddModelStateErrors(innerError.Message);
            }
            if (ex is BusinessLogicException)
            {
                //all business exception be showed to client
                exceptionHandlingResult.ErrorMessage = ex.Message;
                shouldRethrow = false;
            }
            else if (ex is UserVisibleException)
            {
                //exception has been transformed
                exceptionHandlingResult.ErrorMessage = ex.Message;

                shouldRethrow = true;
            }
            else if (ex is DataBadSqlException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataCannotSerializeTransactionException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataDeadlockException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataIntegrityViolationException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataLockingFailureException)
            {
                //exceptionHandlingResult.ErrorMessage =
                //    TranslationService.TranslateString("ConcurrencyExceptionMessageText");

                shouldRethrow = false;
            }

            else if (ex is DataObjectRetrievalFailureException)
            {
                shouldRethrow = false;
            }
            else if (ex is DataPermissionDeniedException)
            {
                shouldRethrow = true;
            }
            else if (ex is DataAccessException)
            {
                shouldRethrow = true;
            }
            else //all other exception
            {
                shouldRethrow = true;
            }

            return shouldRethrow;
        }
        /// <summary>
        ///     Get deployment mode of application, it can  be developement or production
        /// </summary>
        public virtual bool IsProductionMode
        {
            get
            {
                var applicationMode = ConfigurationManager.AppSettings["ApplicationMode"];
                return !string.IsNullOrEmpty(applicationMode) &&
                       String.CompareOrdinal("production", applicationMode.ToLower().Trim()) == 0;
            }
        }

    }
}