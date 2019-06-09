using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.Entities.Mapping
{
    public class CountryOrRegionMap : QuickspatchEntityTypeConfiguration<CountryOrRegion>
    {
        public CountryOrRegionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.CodeName)
                .IsRequired()
                .HasMaxLength(4);
            // Table & Column Mappings
            ToTable("CountryOrRegion");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.CodeName).HasColumnName("CodeName");

        }
    }
}
