using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public class DocumentType : Entity
    {
        public DocumentType()
        {
            UserRoleFunctions = new List<UserRoleFunction>();
            GridConfigs = new Collection<GridConfig>();
        }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public int Order { get; set; }

        public virtual ICollection<GridConfig> GridConfigs { get; set; }

        public virtual ICollection<UserRoleFunction> UserRoleFunctions { get; set; }

        public virtual ICollection<ModuleDocumentTypeOperation> ModuleDocumentTypeOperations { get; set; }
    }
}