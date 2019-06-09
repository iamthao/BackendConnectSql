using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Framework.DomainModel;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Exceptions.DataAccess.Sql;
using Framework.Mapping;
using Framework.Service.Diagnostics;
using Framework.Service.Translation;
using QuickspatchApi.Models;
using ServiceLayer.Interfaces;

namespace QuickspatchApi.Controllers
{
    [OutputCache(Duration = 0, VaryByParam = "*")]
    public abstract class ApplicationControllerGeneric<TEntity, TViewModel> : ApiController
        where TEntity : Entity
        where TViewModel : DtoBase, new()
    {
        //protected readonly IAuthenticationService _authenticationService;
        private readonly IDiagnosticService _diagnosticService;
        private readonly IMasterFileService<TEntity> _masterfileService;
        protected readonly AppSettingsReader AppSettingsReader = new AppSettingsReader();
        protected ApplicationControllerGeneric(IDiagnosticService diagnosticService,
            IMasterFileService<TEntity> masterfileService
            )
        {
            _diagnosticService = diagnosticService;
            _masterfileService = masterfileService;
            // This flag to diable the validate for special characters in the texboxes.
            //ValidateRequest = false;
        }

        protected IMasterFileService<TEntity> MasterFileService
        {
            get { return _masterfileService; }
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
        public virtual IHttpActionResult GetLookupForEntity(LookupQuery query, Func<TEntity, LookupItemVo> selector)
        {
            var queryData = _masterfileService.GetLookup(query, selector);
            return Ok(queryData);
        }

        public virtual IHttpActionResult GetLookupItemForEntity(LookupItem lookupItem, Func<TEntity, LookupItemVo> selector)
        {
            var data = _masterfileService.GetLookupItem(lookupItem, selector);
            return Ok(data);
        }

        protected virtual TViewModel MapFromClientParameters(MasterfileParameter parameters, Action<TViewModel> advanceMapping = null)
        {
            return MapFromClientParameters<TViewModel>(parameters, advanceMapping);
        }

        protected virtual TVModel MapFromClientParameters<TVModel>(MasterfileParameter parameters, Action<TVModel> advanceMapping = null)
            where TVModel : DtoBase, new()
        {
            var viewModel = new TVModel();

            //viewModel.ProcessFromClientParameters(parameters);
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


        [ChildActionOnly]
        public virtual IHttpActionResult DeleteMultiMasterfile(string selectedRowIdArray, string isDeleteAll)
        {
            if (isDeleteAll == "1")
            {
                MasterFileService.DeleteAll(o => o.Id > 0);
            }
            else
            {
                var liststrId = selectedRowIdArray.Split(',');
                var listId = new List<long>();
                foreach (var item in liststrId)
                {
                    long id;
                    long.TryParse(item, out id);
                    if (id != 0)
                    {
                        listId.Add(id);
                    }
                }
                MasterFileService.DeleteAll(o => listId.Contains(o.Id));
            }
            return Ok(new { Error = string.Empty });
        }

        public virtual IHttpActionResult GetDataForGridMasterFile(QueryInfo queryInfo)
        {
            var queryData = MasterFileService.GetDataForGridMasterfile(queryInfo);
            return Ok(queryData);
        }






        [ChildActionOnly]
        public virtual IHttpActionResult DeleteMasterFile(TViewModel viewModel)
        {
            var entity = MasterFileService.GetById(viewModel.Id);
            //entity.LastModified = viewModel.LastModified;
            MasterFileService.Delete(entity);
            return Ok(new { Error = string.Empty });
        }

        [ChildActionOnly]
        protected FeedbackViewModel BuildFeedBackViewModel(Exception ex, IEnumerable<string> modelStateErrors)
        {
            var feedback = new FeedbackViewModel();
            ExceptionHandlingResult exceptionHandlingResult;

            var shouldRethrow = HandleException(ex, out exceptionHandlingResult);

            feedback.Status = shouldRethrow ? FeedbackStatus.Critical : FeedbackStatus.Error;

            feedback.Error = exceptionHandlingResult.ErrorMessage;
            feedback.AddModelStateErrors(modelStateErrors.ToArray());

            //add more exception from exception stack trace
            feedback.AddModelStateErrors(exceptionHandlingResult.ModelStateErrors.ToArray());

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