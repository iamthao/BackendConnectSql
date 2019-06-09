using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class AutomateSendRequestMap : QuickspatchEntityTypeConfiguration<AutomateSendRequest>
    {
        public AutomateSendRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LastModified)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            this.Property(t => t.CronTrigger)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.QuartzName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AutomateSendRequest");
            this.Property(t => t.CronTrigger).HasColumnName("CronTrigger");
            this.Property(t => t.QuartzName).HasColumnName("QuartzName");

            // Relationships
            this.HasRequired(t => t.Request)
                .WithOptional(t => t.AutomateSendRequest);

        }
    }
}
