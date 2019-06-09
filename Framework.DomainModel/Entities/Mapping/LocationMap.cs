using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class LocationMap : QuickspatchEntityTypeConfiguration<Location>
    {
        public LocationMap()
        {
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Address1)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Address2)
                .HasMaxLength(200);

            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Zip)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.AvailableTime)
                .IsFixedLength()
                .HasMaxLength(7);

            //
            this.Property((t => t.StateOrProvinceOrRegion))
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Location");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.AvailableTime).HasColumnName("AvailableTime");
            this.Property(t => t.OpenHour).HasColumnName("OpenHour");
            this.Property(t => t.CloseHour).HasColumnName("CloseHour");
            this.Property(t => t.Lat).HasColumnName("Lat");
            this.Property(t => t.Lng).HasColumnName("Lng");
            this.Property(t => t.AutoGetCityState).HasColumnName("AutoGetCityState");
            //
            this.Property(t => t.StateOrProvinceOrRegion).HasColumnName("StateOrProvinceOrRegion");
            this.Property(t => t.IdCountryOrRegion).HasColumnName("IdCountryOrRegion");
            //this.HasRequired(t => t.State)
            //    .WithMany(t => t.Locations)
            //    .HasForeignKey(d => d.State);
        }
    }
}
