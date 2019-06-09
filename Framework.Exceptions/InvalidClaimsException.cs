using System;
using System.Runtime.Serialization;
using System.Security.Principal;
using Framework.Service.Translation;

namespace Framework.Exceptions
{
    [Serializable]
    public class InvalidClaimsException : QuickspatchException
    {
        public string KeyAuthentication { get; set; }
        public InvalidClaimsException()
        {
        }

        public InvalidClaimsException(string messageResourceKey)
            : base(SystemMessageLookup.GetMessage(messageResourceKey))
        {
        }

        public InvalidClaimsException(string messageResourceKey, string keyAuthentication)
            : base(SystemMessageLookup.GetMessage(messageResourceKey))
        {
            KeyAuthentication = keyAuthentication;
        }
        public InvalidClaimsException(string messageResourceKey, Exception inner)
            : base(SystemMessageLookup.GetMessage(messageResourceKey), inner)
        {
        }


        protected InvalidClaimsException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        public IPrincipal ClaimsPrincipal { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Claims Info", ClaimsPrincipal);
        }
    }
}
