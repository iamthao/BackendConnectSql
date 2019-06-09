using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.DomainModel.Entities.Mapping
{
    public class SecurityOperationMap : QuickspatchEntityTypeConfiguration<SecurityOperation>
    {
        public SecurityOperationMap()
        {
            Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            ToTable("SecurityOperation");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}