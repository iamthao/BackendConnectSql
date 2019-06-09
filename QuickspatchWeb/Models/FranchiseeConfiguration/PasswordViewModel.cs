using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class PasswordViewModel
    {
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}