using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.Entities.Mapping
{
    public class RegisterMap : QuickspatchEntityTypeConfiguration<Register>
    {
        public RegisterMap()
        {
            // Primary Key

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(100);
            this.Property(t => t.CompanyName)
                .HasMaxLength(200);

            this.Property(t => t.DomainName)
                .HasMaxLength(250);

            this.Property(t => t.DatabaseName)
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasMaxLength(100);

            this.Property(t => t.Phone)
                .HasMaxLength(20);

            this.Property(t => t.Password)
                .HasMaxLength(100);


            this.Property(t => t.LicenseKey)
                .HasMaxLength(200);
            this.Property(t => t.ApiDomainName)
                .HasMaxLength(300);
            this.Property(t => t.Username)
                .HasMaxLength(200);
            this.Property(t => t.PasswordHashcode)
                .HasMaxLength(100);
            // Table & Column Mappings
            this.ToTable("Register");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.ApiDomainName).HasColumnName("ApiDomainName");
            this.Property(t => t.DomainName).HasColumnName("DomainName");
            this.Property(t => t.DatabaseName).HasColumnName("DatabaseName");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.PasswordHashcode).HasColumnName("PasswordHashcode");
            this.Property(t => t.IsConfirm).HasColumnName("IsConfirm");
            this.Property(t => t.IsDone).HasColumnName("IsDone");
            this.Property(t => t.LicenseKey).HasColumnName("LicenseKey");
            this.Property(t => t.StartActiveDate).HasColumnName("StartActiveDate");
            this.Property(t => t.EndActiveDate).HasColumnName("EndActiveDate");
            this.Property(t => t.NumberOfCourier).HasColumnName("NumberOfCourier");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.DeploymentId).HasColumnName("DeploymentId");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.MiddleName).HasColumnName("MiddleName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstRequestId).HasColumnName("FirstRequestId");
            this.Property(t => t.FirstPayDate).HasColumnName("FirstPayDate");
            this.Property(t => t.PaymentStatus).HasColumnName("PaymentStatus");
            this.Property(t => t.PaymentMessage).HasColumnName("PaymentMessage");
        }
    }
}
