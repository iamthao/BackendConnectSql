using System;

namespace Framework.Exceptions
{
    [Serializable]
    public class BusinessLogicException :QuickspatchException
    {
        public BusinessLogicException(string message, Exception rootCause)
            : base(message, rootCause)
        {
        }
    }
}