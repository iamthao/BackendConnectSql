using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.DomainModel.Entities.Mapping
{
    public class DocumentTypeMap : QuickspatchEntityTypeConfiguration<DocumentType>
    {
        public DocumentTypeMap()
        {
            Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(1000);

            ToTable("DocumentType");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Type).HasColumnName("Type");
            Property(t => t.Title).HasColumnName("Title");
            Property(t => t.Order).HasColumnName("Order");
        }
    }
}