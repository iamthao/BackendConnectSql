using Framework.Service.Translation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.DataAnnotations
{
    public class LocalizeRequiredAttribute : RequiredAttribute
    {
        public string FieldName { get; set; }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), String.IsNullOrEmpty(FieldName) ? name : FieldName);
        }
    }
}
