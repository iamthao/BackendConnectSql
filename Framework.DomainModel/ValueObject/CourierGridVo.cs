using System.Globalization;
using Framework.DomainModel.Common;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public  class CourierGridVo : ReadOnlyGridVo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }

        public string HomePhoneInFormat { get { return HomePhone.ApplyFormatPhone(); } }

        public string MobilePhoneInFormat { get { return MobilePhone.ApplyFormatPhone(); } }

        public int? Status { get; set; }
        public string StatusString {
            get
            {
                return (Status ??0).GetNameByValue<StatusCourier>().ToLower();
            }
        }
        public string CarNo { get; set; }
        public string Imei { get; set; }
    }
}
