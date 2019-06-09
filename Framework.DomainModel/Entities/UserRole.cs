using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class UserRole : Entity
    {
        public UserRole()
        {
            Users = new List<User>();
            UserRoleFunctions = new List<UserRoleFunction>();
        }

        public string Name { get; set; }

        public string AppRoleName { get; set; }


        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<UserRoleFunction> UserRoleFunctions { get; set; }
    }
}