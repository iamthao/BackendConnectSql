using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class ScheduleMap : QuickspatchEntityTypeConfiguration<Schedule>
    {
        public ScheduleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.LastModified)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.Frequency)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Schedule");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.LocationFrom).HasColumnName("LocationFrom");
            this.Property(t => t.LocationTo).HasColumnName("LocationTo");
            this.Property(t => t.Frequency).HasColumnName("Frequency");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.DurationStart).HasColumnName("DurationStart");
            this.Property(t => t.DurationEnd).HasColumnName("DurationEnd");
            this.Property(t => t.IsNoDurationEnd).HasColumnName("IsNoDurationEnd");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CourierId).HasColumnName("CourierId");
            this.Property(t => t.TimeZone).HasColumnName("TimeZone");
            this.Property(t => t.IsWarning).HasColumnName("IsWarning");
            // Relationships
            this.HasRequired(t => t.LocationFromObj)
                .WithMany(t => t.SchedulesFrom)
                .HasForeignKey(d => d.LocationFrom);
            this.HasRequired(t => t.LocationToObj)
                .WithMany(t => t.SchedulesTo)
                .HasForeignKey(d => d.LocationTo);
            this.HasRequired(t => t.Courier)
                .WithMany(t => t.Schedules)
                .HasForeignKey(d => d.CourierId);

        }
    }
}
