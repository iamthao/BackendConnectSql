using System;
using System.Runtime.Serialization;
using Framework.Service.Translation;

namespace Framework.Exceptions
{
    [Serializable]
    public class UnAuthorizedAccessException : QuickspatchException
    {
        public UnAuthorizedAccessException()
        {
        }

        public UnAuthorizedAccessException(string messageResourceKey)
            : base(SystemMessageLookup.GetMessage(messageResourceKey))
        {
        }

        public UnAuthorizedAccessException(string messageResourceKey, Exception inner)
            : base(SystemMessageLookup.GetMessage(messageResourceKey), inner)
        {
        }

        protected UnAuthorizedAccessException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
