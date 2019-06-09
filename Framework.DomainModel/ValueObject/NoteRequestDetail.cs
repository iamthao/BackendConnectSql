using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.ValueObject
{
    public class NoteRequestDetail
    {
        public DateTime? CreateTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CourierName { get; set; }
        public string Tag { get; set; }
        public bool IsSchedule { get; set; }
    }
}
