using System;

namespace Database.Persistance.ExceptionHandler
{
    public static class DbEntityValidationExceptionHandler
    {
        public static object[] Process(Exception ex)
        {
            return new object[] { "ValidationErrorText", ex };
        }
    }
}
