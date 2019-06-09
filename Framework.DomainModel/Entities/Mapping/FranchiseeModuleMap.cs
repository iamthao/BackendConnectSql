namespace Framework.DomainModel.Entities.Mapping
{
    public class FranchiseeModuleMap : QuickspatchEntityTypeConfiguration<FranchiseeModule>
    {
        public FranchiseeModuleMap()
        {
            // Table & Column Mappings
            this.ToTable("Franchisee_Module");
            this.Property(t => t.FranchiseeId).HasColumnName("FranchiseeId");
            this.Property(t => t.ModuleId).HasColumnName("ModuleId");

            // Relationships
            this.HasRequired(t => t.FranchiseeTenant)
                .WithMany(t => t.FranchiseeModule)
                .HasForeignKey(d => d.FranchiseeId);
            this.HasRequired(t => t.Module)
                .WithMany(t => t.FranchiseeModules)
                .HasForeignKey(d => d.ModuleId);

        }
    }
}