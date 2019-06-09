using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class PaymentInfoApiDto
    {
        public string ProductKey { get; set; }
        public string SecretKey { get; set; }
        public int Id { get; set; }
    }
}
