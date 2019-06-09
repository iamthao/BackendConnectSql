namespace Framework.DomainModel.Entities.Mapping
{
    public class GridConfigMap : QuickspatchEntityTypeConfiguration<GridConfig>
    {
        public GridConfigMap()
        {
            Property(t => t.XmlText)
                .IsRequired();

            Property(t => t.GridInternalName)
                .IsRequired()
                .HasMaxLength(255);

            ToTable("GridConfig");
            Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.XmlText).HasColumnName("XmlText");
            Property(t => t.GridInternalName).HasColumnName("GridInternalName");

            HasRequired(t => t.DocumentType)
                .WithMany(t => t.GridConfigs)
                .HasForeignKey(d => d.DocumentTypeId);

            HasRequired(t => t.User)
                .WithMany(t => t.GridConfigs)
                .HasForeignKey(d => d.UserId);
        }
    }
}