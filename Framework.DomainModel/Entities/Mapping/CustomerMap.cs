namespace Framework.DomainModel.Entities.Mapping
{
    public class CustomerMap : QuickspatchEntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            Property(t => t.Name).IsRequired().HasMaxLength(1000);
            ToTable("Customer");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}