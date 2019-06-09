using System;
using System.Runtime.Serialization;
using Framework.Service.Translation;

namespace Framework.Exceptions
{
    /// <summary>
    ///     Generic translated exception that is visible to users.
    /// </summary>
    /// <remarks>
    ///     By default, exceptions are not displayed to users. Instead, a generic error message is displayed.
    ///     Only exceptions of this type are translated and displayed to
    ///     the users. Be careful not to include any security-relevant internals of the application in exceptions of this type.
    /// </remarks>
    [Serializable]
    public class UserVisibleException : DataAccessException
    {
        public UserVisibleException()
        {
        }

        public UserVisibleException(string messageResourceKey)
            : base(SystemMessageLookup.GetMessage(messageResourceKey))
        {
            MessageResourceKey = messageResourceKey;
        }


        public UserVisibleException(string messageResourceKey, Exception inner)
            : base(SystemMessageLookup.GetMessage(messageResourceKey), inner)
        {
        }

        protected UserVisibleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public string MessageResourceKey { get; set; }
    }
}