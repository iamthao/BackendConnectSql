using Framework.DomainModel.Entities.Security;

namespace Framework.DomainModel.ValueObject
{
    public class ModuleDocumentTypeOperationGridVo : ReadOnlyGridVo
    {
        public string Module { get; set; }
        public string DocumentType { get; set; }
        public int SercurityOperation { get; set; }

        public bool IsView
        {
            get
            {
                return SercurityOperation == (int) OperationAction.View;
            } 
        }
        public bool IsInsert
        {
            get
            {
                return SercurityOperation == (int)OperationAction.Add;
            }
        }
        public bool IsUpdate
        {
            get
            {
                return SercurityOperation == (int)OperationAction.Update;
            }
        }
        public bool IsDelete
        {
            get
            {
                return SercurityOperation == (int)OperationAction.Delete;
            }
        }
        public bool IsProcess
        {
            get
            {
                return SercurityOperation == (int)OperationAction.Process;
            }
        }
    }
}