using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class ContactGridVo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public string PhoneFormat
        {
            get { return Phone.ApplyFormatPhone(); }
        }
    }
}
