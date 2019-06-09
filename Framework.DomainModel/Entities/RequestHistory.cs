using System;

namespace Framework.DomainModel.Entities
{
    public class RequestHistory : Entity
    {
        public int? RequestId { get; set; }
        public int CourierId { get; set; }
        public int ActionType { get; set; }
        public int LastRequestStatus { get; set; }
        public string Comment { get; set; }
        public DateTime TimeChanged { get; set; }
        public virtual Request Request { get; set; }
    }
}