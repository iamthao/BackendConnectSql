using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class Module : Entity
    {
        public Module()
        {
            FranchiseeModules = new List<FranchiseeModule>();
            ModuleDocumentTypeOperations = new List<ModuleDocumentTypeOperation>();
        }
        public string Name { get; set; }
        public virtual ICollection<FranchiseeModule> FranchiseeModules { get; set; }
        public virtual ICollection<ModuleDocumentTypeOperation> ModuleDocumentTypeOperations { get; set; }
    }
}