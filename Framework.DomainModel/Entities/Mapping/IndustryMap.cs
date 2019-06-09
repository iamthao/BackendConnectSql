namespace Framework.DomainModel.Entities.Mapping
{
    public class IndustryMap : QuickspatchEntityTypeConfiguration<Industry>
    {
        public IndustryMap()
        {
            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(200);

            this.Property(t => t.DisplayLabel)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Industry");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.DisplayLabel).HasColumnName("DisplayLabel");
        }
    }
}