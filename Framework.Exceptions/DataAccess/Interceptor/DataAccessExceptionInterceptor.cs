using System;
using Castle.DynamicProxy;
using Framework.Exceptions.DataAccess.Translator;

namespace Framework.Exceptions.DataAccess.Interceptor
{
    public class DataAccessExceptionInterceptor : IDataAccessExceptionInterceptor
    {
        private readonly IExceptionTranslator _exceptionTranslator;

        public DataAccessExceptionInterceptor(IExceptionTranslator exceptionTranslator)
        {
            _exceptionTranslator = exceptionTranslator;
        }

        /// <summary>
        ///     Execute and translate exception to database exception
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                throw _exceptionTranslator.TranslateException(ex);
            }
        }
    }
}
