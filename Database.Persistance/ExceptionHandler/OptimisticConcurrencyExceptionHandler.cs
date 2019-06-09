using System;

namespace Database.Persistance.ExceptionHandler
{
    public static class OptimisticConcurrencyExceptionHandler
    {
        public static object[] Process(Exception ex)
        {
            return new object[] { "ConcurrencyExceptionMessageText", null };
        }
    }
}