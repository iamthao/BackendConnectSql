using System.Collections.Generic;

namespace QuickspatchApi.Models
{
    public class ExceptionHandlingResult
    {
        public ExceptionHandlingResult()
        {
            ModelStateErrors = new List<string>();
        }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        /// <summary>
        ///     Contains an array of validation errors on the current form.
        /// </summary>
        public IList<string> ModelStateErrors { get; set; }

        /// <summary>
        ///     Add a list of errors to model state
        /// </summary>
        /// <param name="error"></param>
        public void AddModelStateError(string error)
        {
            ModelStateErrors.Add(error);
        }

        /// <summary>
        ///     Add errors to model state
        /// </summary>
        /// <param name="errors"></param>
        public void AddModelStateErrors(params string[] errors)
        {
            foreach (var error in errors)
            {
                ModelStateErrors.Add(error);
            }
        }
    }
}