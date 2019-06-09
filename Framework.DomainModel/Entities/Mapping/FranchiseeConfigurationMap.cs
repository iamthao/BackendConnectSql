namespace Framework.DomainModel.Entities.Mapping
{
    public class FranchiseeConfigurationMap : QuickspatchEntityTypeConfiguration<FranchiseeConfiguration>
    {
        public FranchiseeConfigurationMap()
        {
            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);


            this.Property(t => t.Address1)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.OfficePhone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Address2)
                .HasMaxLength(25);

            this.Property(t => t.FaxNumber)
                .HasMaxLength(20);

            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.Zip)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.State)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LicenseKey)
                .HasMaxLength(200);
            this.Property(t => t.FranchiseeContact)
                .HasMaxLength(200);
            this.Property(t => t.PrimaryContactPhone)
                .HasMaxLength(20);
            this.Property(t => t.PrimaryContactEmail)
                .HasMaxLength(50);
            this.Property(t => t.PrimaryContactFax)
                .HasMaxLength(20);
            this.Property(t => t.PrimaryContactCellNumber)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("FranchiseeConfiguration");
            this.Property(t => t.FranchiseeId).HasColumnName("FranchiseeId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.FaxNumber).HasColumnName("FaxNumber");
            this.Property(t => t.OfficePhone).HasColumnName("OfficePhone");
            this.Property(t => t.LicenseKey).HasColumnName("LicenseKey");
            this.Property(t => t.FranchiseeContact).HasColumnName("FranchiseeContact");
            this.Property(t => t.PrimaryContactPhone).HasColumnName("PrimaryContactPhone");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.PrimaryContactFax).HasColumnName("PrimaryContactFax");
            this.Property(t => t.PrimaryContactCellNumber).HasColumnName("PrimaryContactCellNumber");
            this.Property(t => t.Logo).HasColumnName("Logo");
            this.Property(t => t.IsQuickTour).HasColumnName("IsQuickTour");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
            this.Property(t => t.LocationFromId).HasColumnName("LocationFromId");
            this.Property(t => t.LocationToId).HasColumnName("LocationToId");
        }
    }
}