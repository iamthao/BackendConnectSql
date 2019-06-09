using System;
using System.Collections.Generic;
using System.Linq;
using Framework.DomainModel.DataTransferObject;
using Framework.Service.Diagnostics;

namespace QuickspatchApi.Controllers
{
    public class ErrorController : ApplicationControllerBase
    {
        private readonly IDiagnosticService _diagnosticService;
        public ErrorController(IDiagnosticService diagnosticService)
            : base(diagnosticService, null)
        {
            _diagnosticService = diagnosticService;
        }


        public virtual FeedbackViewModel GetJsonError(Exception ex, IEnumerable<string> errors)
        {
            var modelStateErrors = errors as IList<string> ?? errors.ToList();
            var feedbackViewModel = BuildFeedBackViewModel(ex, modelStateErrors);
            return feedbackViewModel;
        }
    }
}