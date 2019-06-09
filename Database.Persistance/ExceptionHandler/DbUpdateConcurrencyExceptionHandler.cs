using System;

namespace Database.Persistance.ExceptionHandler
{
    public static class DbUpdateConcurrencyExceptionHandler
    {
        public static object[] Process(Exception ex)
        {
            return new object[] { "ConcurrencyExceptionMessageText", null };
        }
    }
}