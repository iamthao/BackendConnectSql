using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class CourierDto : DtoBase
    {
        public string CarNo { get; set; }
        public UserDto User { get; set; }
        public int CurrentReq { get; set; }
        public bool IsSameImei { get; set; }
        public bool IsExpire { get; set; }
        public List<ContactDto> Contacts { get; set; }
    }
}
