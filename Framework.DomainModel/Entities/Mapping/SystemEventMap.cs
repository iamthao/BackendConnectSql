using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class SystemEventMap : QuickspatchEntityTypeConfiguration<SystemEvent>
    {
        public SystemEventMap()
        {

            Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable("SystemEvent");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.EventType).HasColumnName("EventType");
        }
    }
}
