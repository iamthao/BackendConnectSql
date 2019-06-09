using Framework.Service.Translation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.DataAnnotations
{
    public class LocalizePhoneAttribute : RegularExpressionAttribute
    {
        public string FieldName { get; set; }
        public LocalizePhoneAttribute()
            : base(@"(^\s*$)|(^\d{10}$)|(^[(]\d{3}[)]\s\d{3}[-]\d{4}$)")
            //: base(@"^[0-9]{10}$")
        {

        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(SystemMessageLookup.GetMessage("PhoneValid"), String.IsNullOrEmpty(FieldName) ? name : FieldName);
        }
    }
}
