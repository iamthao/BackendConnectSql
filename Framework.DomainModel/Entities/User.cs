using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Framework.DataAnnotations;

namespace Framework.DomainModel.Entities
{
    public class User : Entity
    {
        public User()
        {
            GridConfigs = new List<GridConfig>();
        }

        [LocalizeRequired(FieldName = "Username")]
        public string UserName { get; set; }

        public string Password { get; set; }

        public int? UserRoleId { get; set; }

        public bool IsActive { get; set; }

        [LocalizeRequired(FieldName = "First Name")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [LocalizeRequired(FieldName = "Last Name")]
        public string LastName { get; set; }

        [LocalizeRequired(FieldName = "Home Phone")]
        //[LocalizePhone(FieldName = "Home Phone")]
        public string HomePhone { get; set; }

        [LocalizeRequired(FieldName = "Mobile Phone")]
        //[LocalizePhone(FieldName = "Mobile Phone")]
        public string MobilePhone { get; set; }

        [LocalizeRequired]
        [LocalizeEmailAddress]
        public string Email { get; set; }

        public byte[] Avatar { get; set; }

        public virtual ICollection<GridConfig> GridConfigs { get; set; }

        public virtual UserRole UserRole { get; set; }

        public virtual Courier Courier { get; set; }
    
        [NotMapped]
        public bool IsQuickspatchUser { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return Utility.CaculatorHelper.GetFullName(FirstName,MiddleName,LastName);
            }
        }
    }
}