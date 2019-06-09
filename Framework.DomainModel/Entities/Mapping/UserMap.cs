namespace Framework.DomainModel.Entities.Mapping
{
    public class UserMap : QuickspatchEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            Property(t => t.UserName).IsRequired().HasMaxLength(100);
            Property(t => t.Password).IsRequired().HasMaxLength(100);
            Property(t => t.FirstName).IsRequired().HasMaxLength(50);
            Property(t => t.MiddleName).HasMaxLength(50);
            Property(t => t.LastName).IsRequired().HasMaxLength(50);
            Property(t => t.HomePhone).IsRequired().HasMaxLength(20);
            Property(t => t.MobilePhone).IsRequired().HasMaxLength(20);
            Property(t => t.Email).IsRequired().HasMaxLength(100);


            ToTable("User");
            Property(t => t.UserName).HasColumnName("UserName");
            Property(t => t.Password).HasColumnName("Password");
            Property(t => t.UserRoleId).HasColumnName("UserRoleId");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.MiddleName).HasColumnName("MiddleName");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.HomePhone).HasColumnName("HomePhone");
            Property(t => t.MobilePhone).HasColumnName("MobilePhone");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.Avatar).HasColumnName("Avatar");
            

            HasOptional(t => t.UserRole).WithMany(t => t.Users).HasForeignKey(d => d.UserRoleId);
        }
    }
}
