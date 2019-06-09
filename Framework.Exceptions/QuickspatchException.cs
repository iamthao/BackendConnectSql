using System;
using System.Runtime.Serialization;

namespace Framework.Exceptions
{
    public class QuickspatchException: Exception
    {

        public string QuickspatchUserName { get; set; }
        /// <summary>
        ///     Initializes a new instance of the <see cref="QuickspatchException" /> class.
        /// </summary>
        public QuickspatchException()
        {
         
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuickspatchException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        public QuickspatchException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuickspatchException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="inner">
        ///     The root cause.
        /// </param>
        public QuickspatchException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuickspatchException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The serialization information.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        public QuickspatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}