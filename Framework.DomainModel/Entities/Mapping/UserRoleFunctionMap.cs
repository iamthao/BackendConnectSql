namespace Framework.DomainModel.Entities.Mapping
{
    public class UserRoleFunctionMap : QuickspatchEntityTypeConfiguration<UserRoleFunction>
    {
        public UserRoleFunctionMap()
        {
            ToTable("UserRoleFunction");
            Property(t => t.UserRoleId).HasColumnName("UserRoleId");
            Property(t => t.SecurityOperationId).HasColumnName("SecurityOperationId");
            Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");

            HasRequired(t => t.DocumentType)
                .WithMany(t => t.UserRoleFunctions)
                .HasForeignKey(d => d.DocumentTypeId);
            HasRequired(t => t.SecurityOperation)
                .WithMany(t => t.UserRoleFunctions)
                .HasForeignKey(d => d.SecurityOperationId);
            HasRequired(t => t.UserRole)
                .WithMany(t => t.UserRoleFunctions)
                .HasForeignKey(d => d.UserRoleId);
        }
    }
}