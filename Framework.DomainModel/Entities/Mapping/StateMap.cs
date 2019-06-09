using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class StateMap : QuickspatchEntityTypeConfiguration<State>
    {
        public StateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.AbbreviationName)
                .HasMaxLength(5);


            // Table & Column Mappings
            ToTable("State");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.AbbreviationName).HasColumnName("AbbreviationName");

        }
    }
}
