namespace Framework.DomainModel.Entities.Mapping
{
    public class UserRoleMap : QuickspatchEntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            Property(t => t.Name).IsRequired().HasMaxLength(50);
            Property(t => t.AppRoleName).HasMaxLength(50);

            ToTable("UserRole");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.AppRoleName).HasColumnName("AppRoleName");
        }
    }
}