namespace Framework.DomainModel.Entities
{
    public class FranchiseeModule : Entity
    {
        public int FranchiseeId { get; set; }
        public int ModuleId { get; set; }
        public virtual FranchiseeTenant FranchiseeTenant { get; set; }
        public virtual Module Module { get; set; }
    }
}