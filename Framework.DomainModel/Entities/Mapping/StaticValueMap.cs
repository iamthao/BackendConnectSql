using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class StaticValueMap : QuickspatchEntityTypeConfiguration<StaticValue>
    {
        public StaticValueMap()
        {




            // Table & Column Mappings
            this.ToTable("StaticValue");
            this.Property(t => t.RequestNumber).HasColumnName("RequestNumber");

        }
    }
}
