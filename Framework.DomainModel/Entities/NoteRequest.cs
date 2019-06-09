using System;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class NoteRequest : Entity
    {
        public int RequestId { get; set; }
        public int? CourierId { get; set; }
        public string Description { get; set; }
        public virtual Courier Courier { get; set; }
        public virtual Request Request { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
