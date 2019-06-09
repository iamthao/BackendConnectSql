using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class PackageHistoryMap : QuickspatchEntityTypeConfiguration<PackageHistory>
    {
        public PackageHistoryMap()
        {
            // Table & Column Mappings
            ToTable("PackageHistory");
            Property(t => t.PackageId).HasColumnName("PackageId").IsRequired();
            Property(t => t.OldPackageId).HasColumnName("OldPackageId");
            Property(t => t.StartDate).HasColumnName("StartDate").IsRequired();
            Property(t => t.EndDate).HasColumnName("EndDate");
            Property(t => t.RequestId).HasColumnName("RequestId").IsRequired();
            Property(t => t.FranchiseeTenantId).HasColumnName("FranchiseeTenantId").IsRequired();
            Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            Property(t => t.IsApply).HasColumnName("IsApply");
            //Relation Shift
            HasRequired(o => o.FranchiseeTenant)
                .WithMany(o => o.PackageHistories)
                .HasForeignKey(o => o.FranchiseeTenantId);
        }
    }
}
