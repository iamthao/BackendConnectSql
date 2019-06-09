namespace Framework.DomainModel.ValueObject
{
    public class MenuViewModel
    {
        public bool CanViewUserSetup { get; set; }
        public bool CanViewUserRoleSetup { get; set; }
        public bool CanViewModuleSetup { get; set; }
        public bool CanViewFranchiseeSetup { get; set; }
        public bool CanViewSchedule { get; set; }
        public bool CanViewRequest { get; set; }
        public bool CanViewTracking { get; set; }
        public bool CanViewLocation { get; set; }
        public bool CanViewCourier { get; set; }
        public bool CanViewDashboard{ get; set; }
    }
}