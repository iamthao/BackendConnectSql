using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.Entities.Common
{
    public class UserQueryInfo :QueryInfo
    {
        public int CurrentUserId { get; set; }
    }
}
