namespace Framework.DomainModel.Entities
{
    public class ModuleDocumentTypeOperation : Entity
    {
        public int DocumentTypeId { get; set; }
        public int ModuleId { get; set; }
        public int SercurityOperationId { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual Module Module { get; set; }
        public virtual SecurityOperation SecurityOperation { get; set; }
    }
}