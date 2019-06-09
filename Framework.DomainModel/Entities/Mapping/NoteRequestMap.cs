using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class NoteRequestMap : QuickspatchEntityTypeConfiguration<NoteRequest>
    {
        public NoteRequestMap()
        {
            this.Property(t => t.Description)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("NoteRequest");
            this.Property(t => t.RequestId).HasColumnName("RequestId");
            this.Property(t => t.CourierId).HasColumnName("CourierId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");

            // Relationships
            this.HasOptional(t => t.Courier)
                .WithMany(t => t.NoteRequests)
                .HasForeignKey(d => d.CourierId);
            this.HasRequired(t => t.Request)
                .WithMany(t => t.NoteRequests)
                .HasForeignKey(d => d.RequestId);

        }
    }
}
