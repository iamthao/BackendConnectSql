namespace QuickspatchWeb.Models.User
{
    public class DashboardUserShareViewModel : DashboardSharedViewModel
    {
        public string FirstName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string HomePhone { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public int UserRoleId { get; set; }

        public bool IsActive { get; set; }

        //public bool IsQuickspatchUser { get; set; }

    }
}