using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.ValueObject
{
    public class SystemEventGridVo : ReadOnlyGridVo
    {
        public string DescriptionOld { get; set; }
        public string Title { get; set; }
        public string Description {
            get
            {
                return DescriptionOld;
            }
        }
        public DateTime? CreatedOnDateTime { get; set; }

        public bool IsNew
        {
            get { return CreatedOnDateTime.GetValueOrDefault().AddMinutes(1) > DateTime.UtcNow; }
        }
    }
}
