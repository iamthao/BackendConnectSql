using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class ContactDto : DtoBase
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
