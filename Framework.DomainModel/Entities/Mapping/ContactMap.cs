using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class ContactMap : QuickspatchEntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            Property(t => t.Phone)
                .HasMaxLength(20);


            // Table & Column Mappings
            ToTable("Contact");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Phone).HasColumnName("Phone");

        }
    }
}
