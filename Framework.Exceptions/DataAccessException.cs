using System;
using System.Runtime.Serialization;

namespace Framework.Exceptions
{
    /// <summary>
    ///     The data access exception.
    /// </summary>
    [Serializable]
    public class DataAccessException : QuickspatchException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuickspatchException" /> class.
        /// </summary>
        public DataAccessException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuickspatchException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        public DataAccessException(string message)
            : base(message)
        {
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="DataAccessException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="rootCause">
        ///     The root cause.
        /// </param>
        public DataAccessException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="QuickspatchException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The serialization information.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        public DataAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}