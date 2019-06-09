using System.ComponentModel.DataAnnotations;
using Framework.Service.Translation;

namespace Framework.DataAnnotations
{
    public class LocalizeMaxLengthAttribute : MaxLengthAttribute
    {
        public string FieldName { get; set; }
        public LocalizeMaxLengthAttribute(int length):base(length)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            if (!string.IsNullOrEmpty(FieldName))
            {
                name = FieldName;
            }
            return string.Format(SystemMessageLookup.GetMessage("MaxLengthRequied"), name, Length);
        }
    }
}
