using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.Common;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class TemplateGridVo :ReadOnlyGridVo
    {
        public string Title { get; set; }
        public int TemplateTypeId { get; set; }

        public string TemplateType
        {
            get { return ((TemplateType)TemplateTypeId).GetDescription(); }
        }
    }
}
