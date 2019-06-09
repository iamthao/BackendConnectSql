using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.ValueObject
{
    public class NoteRequestGridVo: ReadOnlyGridVo
    {
        public string Description { get; set; }
        public string FirstNameCreatedBy { get; set; }
        public string LastNameCreatedBy { get; set; }
        public string MiddleNameCreatedBy { get; set; }
        public string CreatedBy {
            get
            {
                return Framework.Utility.CaculatorHelper.GetFullName(FirstNameCreatedBy, MiddleNameCreatedBy,
                    LastNameCreatedBy);
            }
        }
        public DateTime? CreatedDateNoFormat { get; set; }

        public string CreatedDate
        {
            get
            {
                if (CreatedDateNoFormat != null)
                    return ((DateTime)CreatedDateNoFormat).ToString("MM/dd/yyyy hh:mm tt");
                return "";
            }
        }
    }
}
