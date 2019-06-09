using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class UserGridVo : ReadOnlyGridVo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Role { get; set; }

        public int? UserRoleId { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public byte[] Avatar { get; set; }

        public bool IsQuickspatchUser { get; set; }
    
        public string FullNameSearch { get; set; }
        public string FullName
        {
            get { return Framework.Utility.CaculatorHelper.GetFullName(FirstName, MiddleName, LastName); }
        }
        public bool IsActive { get; set; }

        public string HomePhoneInFormat { get { return HomePhone.ApplyFormatPhone(); } }

        public string MobilePhoneInFormat { get { return MobilePhone.ApplyFormatPhone(); } }
    }
}