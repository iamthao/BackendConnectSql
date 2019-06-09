using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class ConfigFranchiseeMap : QuickspatchEntityTypeConfiguration<ConfigFranchisee>
    {
        public ConfigFranchiseeMap()
        {
            

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);
            Property(t => t.ConfigType).IsRequired();


            // Table & Column Mappings
            ToTable("ConfigFranchisee");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Value).HasColumnName("Value");
            Property(t => t.ConfigType).HasColumnName("ConfigType");
        }
    }
}
