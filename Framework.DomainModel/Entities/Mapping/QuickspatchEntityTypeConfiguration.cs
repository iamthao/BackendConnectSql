using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class QuickspatchEntityTypeConfiguration<T> : EntityTypeConfiguration<T>
        where T : Entity
    {
        public QuickspatchEntityTypeConfiguration()
        {
            // Primary Key
            HasKey(t => t.Id);
            Property(t => t.LastModified)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();
            Property(t => t.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.CreatedById).HasColumnName("CreatedById");
            Property(t => t.LastUserId).HasColumnName("LastUserId");
            Property(t => t.LastTime).HasColumnName("LastTime");
            Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            Property(t => t.LastModified).HasColumnName("LastModified");

            // Relationships
            HasOptional(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(d => d.CreatedById);
            HasOptional(t => t.LastUser)
                .WithMany()
                .HasForeignKey(d => d.LastUserId);
        }
    }
}