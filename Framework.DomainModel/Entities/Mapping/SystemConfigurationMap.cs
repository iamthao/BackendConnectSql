using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class SystemConfigurationMap : QuickspatchEntityTypeConfiguration<SystemConfiguration>
    {
        public SystemConfigurationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(500);

            Property(t => t.Value)
                .HasMaxLength(2000);


            // Table & Column Mappings
            ToTable("SystemConfiguration");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Value).HasColumnName("Value");
            Property(t => t.DataType).HasColumnName("DataType");
            Property(t => t.SystemConfigType).HasColumnName("SystemConfigType");

        }
    }
}
