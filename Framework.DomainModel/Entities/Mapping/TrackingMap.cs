using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class TrackingMap : QuickspatchEntityTypeConfiguration<Tracking>
    {
        public TrackingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.LastModified)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            this.Property(t => t.Address)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Tracking");
            this.Property(t => t.CourierId).HasColumnName("CourierId");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Lat).HasColumnName("Lat");
            this.Property(t => t.Lng).HasColumnName("Lng");
            this.Property(t => t.Distance).HasColumnName("Distance");
            this.Property(t => t.TimeTracking).HasColumnName("TimeTracking");
            this.Property(t => t.Direction).HasColumnName("Direction");
            this.Property(t => t.Velocity).HasColumnName("Velocity");

            // Relationships
            this.HasRequired(t => t.Courier)
                .WithMany(t => t.Trackings)
                .HasForeignKey(d => d.CourierId);
            this.HasOptional(t => t.Request)
                .WithMany(t => t.Trackings)
                .HasForeignKey(d => d.RequestId);

        }
    }
}
