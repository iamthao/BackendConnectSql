using System;
using System.Data;
using System.Data.Entity.Core;
using Framework.Exceptions.DataAccess.Meta;
using Framework.Exceptions.DataAccess.Sql;

namespace Framework.Exceptions.DataAccess.Translator
{
    public class EntityFrameworkExceptionTranslator : ExceptionTranslator
    {
        public EntityFrameworkExceptionTranslator(IDbMetaInfo meta)
            : base(meta)
        {
        }

        /// <summary>
        ///     Translate an exception.
        /// </summary>
        /// <param name="ex">
        ///     The exception to translate.
        /// </param>
        /// <returns>
        ///     The translated exception.
        /// </returns>
        public override Exception TranslateException(Exception ex)
        {
            //Command compilation occurs when the Entity Framework creates the command tree
            //to represent a store query
            if (ex is EntityCommandCompilationException)
            {
                return new DataObjectRetrievalFailureException(ex.Message, ex);
            }

            //Represents errors that occur when the underlying storage provider could not execute the specified command. 
            //This exception usually wraps a provider-specific exception
            if (ex is EntityCommandExecutionException)
            {
                return new DataObjectRetrievalFailureException(ex.Message, ex);
            }

            //EntitySqlException is thrown when your Entity SQL expression cannot be parsed or processed.
            if (ex is EntitySqlException)
            {
                return new DataObjectRetrievalFailureException(ex.Message, ex);
            }

            //The exception that is thrown to indicate that a command tree is invalid
            if (ex is InvalidCommandTreeException)
            {
                return new DataObjectRetrievalFailureException(ex.Message, ex);
            }

            //The exception that is thrown when an object is not present.
            //cause by function  GetObjectByKey 
            if (ex is ObjectNotFoundException)
            {
                return new DataObjectRetrievalFailureException(ex.Message, ex);
            }

            if (ex is OptimisticConcurrencyException)
            {
                return new DataOptimisticLockingFailureException(ex.Message, ex);
            }

            if (ex is UpdateException)
            {
                return new DataObjectRetrievalFailureException(ex.Message, ex);
            }


            if (ex is EntityException)
            {
                return base.TranslateException(ex.InnerException);
            }

            /*
            //could not connect to database
            if (ex is ArgumentException) 
            {
            }*/

            return base.TranslateException(ex);
        }
    }
}