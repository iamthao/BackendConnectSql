using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class TableVersionMap : QuickspatchEntityTypeConfiguration<TableVersion>
    {
        public TableVersionMap()
        {
            Property(t => t.TableId)
                .IsRequired();

            Property(t => t.Version)
                .HasMaxLength(36)
                .IsRequired();


            // Table & Column Mappings
            ToTable("TableVersion");
            Property(t => t.TableId).HasColumnName("TableId");
            Property(t => t.Version).HasColumnName("Version");

        }
    }
}
