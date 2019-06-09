namespace Framework.DomainModel.Entities.Mapping
{
    public class RequestHistoryMap : QuickspatchEntityTypeConfiguration<RequestHistory>
    {
        public RequestHistoryMap()
        {
            this.Property(t => t.Comment)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("RequestHistory");
            this.Property(t => t.RequestId).HasColumnName("RequestId");
            this.Property(t => t.CourierId).HasColumnName("CourierId");
            this.Property(t => t.ActionType).HasColumnName("ActionType");
            this.Property(t => t.LastRequestStatus).HasColumnName("LastRequestStatus");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.TimeChanged).HasColumnName("TimeChanged");

            // Relationships
            this.HasOptional(t => t.Request)
                .WithMany(t => t.RequestHistorys)
                .HasForeignKey(d => d.RequestId);
        }
    }
}