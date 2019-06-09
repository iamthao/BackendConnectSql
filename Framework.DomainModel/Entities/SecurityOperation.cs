using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public class SecurityOperation : Entity
    {
        public SecurityOperation()
        {
            UserRoleFunctions = new List<UserRoleFunction>();
            ModuleDocumentTypeOperations = new Collection<ModuleDocumentTypeOperation>();
        }

        public string Name { get; set; }

        public virtual ICollection<UserRoleFunction> UserRoleFunctions { get; set; }

        public virtual ICollection<ModuleDocumentTypeOperation> ModuleDocumentTypeOperations { get; set; }
    }
}