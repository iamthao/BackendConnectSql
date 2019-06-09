using System;
using Framework.DomainModel.DataTransferObject;

namespace Framework.Exceptions
{
    public class WebApiErrorException : Exception
    {
        private readonly FeedbackViewModel _feedbackViewModel;

        public WebApiErrorException(FeedbackViewModel objFeedbackViewModel)
        {
            _feedbackViewModel = objFeedbackViewModel;
        }

        public FeedbackViewModel ErrorData { get { return _feedbackViewModel; } }
    }
}