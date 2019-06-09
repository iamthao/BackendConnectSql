namespace Framework.DomainModel.Entities.Mapping
{
    public class ModuleMap : QuickspatchEntityTypeConfiguration<Module>
    {
        public ModuleMap()
        {
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Module");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}