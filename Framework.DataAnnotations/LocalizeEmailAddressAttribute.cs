using System.ComponentModel.DataAnnotations;
using Framework.Service.Translation;

namespace Framework.DataAnnotations
{
    public class LocalizeEmailAddressAttribute : RegularExpressionAttribute
    {
        public string FieldName { get; set; }
        public LocalizeEmailAddressAttribute()
            : base(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z")
        {
            
        }

        public override string FormatErrorMessage(string name)
        {
            if (!string.IsNullOrEmpty(FieldName))
            {
                name = FieldName;
            }
            return string.Format(SystemMessageLookup.GetMessage("EmailValid"), name);
        }
    }
}
