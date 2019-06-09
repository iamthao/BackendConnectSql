using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class HoldingRequestMap : QuickspatchEntityTypeConfiguration<HoldingRequest>
    {
        public HoldingRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.LastModified)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            this.Property(t => t.Description)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("HoldingRequest");
            this.Property(t => t.LocationFrom).HasColumnName("LocationFrom");
            this.Property(t => t.LocationTo).HasColumnName("LocationTo");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.SendDate).HasColumnName("SendDate");

            // Relationships
            this.HasRequired(t => t.Location)
                .WithMany(t => t.HoldingRequestsFrom)
                .HasForeignKey(d => d.LocationFrom);
            this.HasRequired(t => t.Location1)
                .WithMany(t => t.HoldingRequestsTo)
                .HasForeignKey(d => d.LocationTo);

        }
    }
}
