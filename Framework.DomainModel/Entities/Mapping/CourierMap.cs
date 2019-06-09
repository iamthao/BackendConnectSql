using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class CourierMap : QuickspatchEntityTypeConfiguration<Courier>
    {
        public CourierMap()
        {

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            this.Property(t => t.CarNo)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Courier");
            this.Property(t => t.Status).HasColumnName("Status");
            Property(t => t.Imei).HasColumnName("Imei");
            Property(t => t.CurrentReq).HasColumnName("CurrentReq");
            Property(t => t.CarNo).HasColumnName("CarNo");
            Property(t => t.ServiceResetTime).HasColumnName("ServiceResetTime");
            Property(t => t.CurrentLat).HasColumnName("CurrentLat");
            Property(t => t.CurrentLng).HasColumnName("CurrentLng");
            Property(t => t.CurrentVelocity).HasColumnName("CurrentVelocity");

            // Relationships
            this.HasRequired(t => t.User)
                .WithOptional(t => t.Courier);
        }
    }
}
