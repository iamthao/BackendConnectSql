namespace Framework.DomainModel.Entities.Mapping
{
    public class ModuleDocumentTypeOperationMap : QuickspatchEntityTypeConfiguration<ModuleDocumentTypeOperation>
    {
        public ModuleDocumentTypeOperationMap()
        {
            // Table & Column Mappings
            this.ToTable("Module_DocumentType_Operation");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.ModuleId).HasColumnName("ModuleId");
            this.Property(t => t.SercurityOperationId).HasColumnName("SercurityOperationId");

            // Relationships
            this.HasRequired(t => t.DocumentType)
                .WithMany(t => t.ModuleDocumentTypeOperations)
                .HasForeignKey(d => d.DocumentTypeId);
            this.HasRequired(t => t.Module)
                .WithMany(t => t.ModuleDocumentTypeOperations)
                .HasForeignKey(d => d.ModuleId);
            this.HasRequired(t => t.SecurityOperation)
                .WithMany(t => t.ModuleDocumentTypeOperations)
                .HasForeignKey(d => d.SercurityOperationId);

        }
    }
}