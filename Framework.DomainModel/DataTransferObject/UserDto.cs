using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.DataTransferObject
{
    public class UserDto : DtoBase
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string HomePhone { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public long? UserRoleId { get; set; }

        public string Password { get; set; }

        public string Avatar { get; set; }

        public string AvatarInBase64 { get; set; }
    }
}
