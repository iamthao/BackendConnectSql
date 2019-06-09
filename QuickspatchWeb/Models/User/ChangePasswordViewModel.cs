using System;
using Framework.DataAnnotations;

namespace QuickspatchWeb.Models.User
{
    public class ChangePasswordViewModel
    {
        public int Id { get; set; }
        [LocalizeRequired]
        public string Username { get; set; }
        [LocalizeRequired]
        public string CurrentPassword { get; set; }
        [LocalizeRequired]
        public string NewPassword { get; set; }
        [LocalizeRequired]
        public string ConfirmNewPassword { get; set; }
    }

    public class UnAuthenticatedChangePasswordViewModel
    {
        public string NewPassword { get; set; }

        public Guid Token { get; set; }
    }
}