namespace Framework.DomainModel.Entities.Mapping
{
    public class FranchiseeTenantMap : QuickspatchEntityTypeConfiguration<FranchiseeTenant>
    {
        public FranchiseeTenantMap()
        {
            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Server)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Database)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Address1)
                .HasMaxLength(25);

            this.Property(t => t.OfficePhone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Address2)
                .HasMaxLength(25);

            this.Property(t => t.FaxNumber)
                .HasMaxLength(20);

            this.Property(t => t.City)
                .HasMaxLength(25);

            this.Property(t => t.Zip)
                .HasMaxLength(10);

            this.Property(t => t.State)
                .HasMaxLength(50);

            this.Property(t => t.LicenseKey)
                .HasMaxLength(200);

            this.Property(t => t.LicenseKey)
                .HasMaxLength(200);
            

            // Table & Column Mappings
            this.ToTable("FranchiseeTenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Server).HasColumnName("Server");
            this.Property(t => t.Database).HasColumnName("Database");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.OfficePhone).HasColumnName("OfficePhone");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.FaxNumber).HasColumnName("FaxNumber");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.LicenseKey).HasColumnName("LicenseKey");
            this.Property(t => t.StartActiveDate).HasColumnName("StartActiveDate");
            this.Property(t => t.EndActiveDate).HasColumnName("EndActiveDate");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.NumberOfCourier).HasColumnName("NumberOfCourier");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
            this.Property(t => t.CurrentPackageId).HasColumnName("CurrentPackageId");
            this.Property(t => t.PaymentStatus).HasColumnName("PaymentStatus");
            this.Property(t => t.PaymentMessage).HasColumnName("PaymentMessage");
            Property(t => t.TotalNotificationTrial).HasColumnName("TotalNotificationTrial");
            Property(t => t.TotalNotificationSuccess).HasColumnName("TotalNotificationSuccess");
            Property(t => t.TotalNotificationError).HasColumnName("TotalNotificationError");
            Property(t => t.TotalNotificationBeforPayment).HasColumnName("TotalNotificationBeforPayment");
            Property(t => t.CloseDate).HasColumnName("CloseDate");
            Property(t => t.QuestionClose).HasColumnName("QuestionClose");
            Property(t => t.DescriptionClose).HasColumnName("DescriptionClose");
            Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            Property(t => t.RemainingAmount).HasColumnName("RemainingAmount");
            Property(t => t.NextBillingDate).HasColumnName("NextBillingDate");
            Property(t => t.PackageNextBillingDate).HasColumnName("PackageNextBillingDate");
            Property(t => t.AlertExtendedPackage).HasColumnName("AlertExtendedPackage");
        }
    }
}