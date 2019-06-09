using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class RequestMap : QuickspatchEntityTypeConfiguration<Request>
    {
        public RequestMap()
        {
            // Properties

            this.Property(t => t.RequestNo)
                .IsRequired()
                .HasMaxLength(20);


            // Table & Column Mappings
            this.ToTable("Request");
            this.Property(t => t.RequestNo).HasColumnName("RequestNo");
            this.Property(t => t.LocationFrom).HasColumnName("LocationFrom");
            this.Property(t => t.LocationTo).HasColumnName("LocationTo");
            this.Property(t => t.CourierId).HasColumnName("CourierId");
            this.Property(t => t.IsStat).HasColumnName("IsStat");
            this.Property(t => t.SendingTime).HasColumnName("SendingTime");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.ReceivedTime).HasColumnName("ReceivedTime");
            this.Property(t => t.AcceptedTime).HasColumnName("AcceptedTime");
            this.Property(t => t.RejectedTime).HasColumnName("RejectedTime");
            this.Property(t => t.ActualStartTime).HasColumnName("ActualStartTime");
            this.Property(t => t.ActualEndTime).HasColumnName("ActualEndTime");
            this.Property(t => t.HistoryScheduleId).HasColumnName("HistoryScheduleId");
            this.Property(t => t.PriorityNumber).HasColumnName("PriorityNumber");
            this.Property(t => t.ExpiredTime).HasColumnName("ExpiredTime");
            this.Property(t => t.IsExpired).HasColumnName("IsExpired");
            this.Property(t => t.IsAgreed).HasColumnName("IsAgreed");
            this.Property(t => t.Signature).HasColumnName("Signature");
            this.Property(t => t.CompletePicture).HasColumnName("CompletePicture");
            this.Property(t => t.CompleteNote).HasColumnName("CompleteNote");
            this.Property(t => t.EstimateDistance).HasColumnName("EstimateDistance");
            this.Property(t => t.EstimateTime).HasColumnName("EstimateTime");
            this.Property(t => t.IsWarning).HasColumnName("IsWarning");
            this.Property(t => t.DistanceEndFrom).HasColumnName("DistanceEndFrom");
            this.Property(t => t.TimeEndFrom).HasColumnName("TimeEndFrom");

            // Relationships
            this.HasOptional(t => t.Courier)
                .WithMany(t => t.Requests)
                .HasForeignKey(d => d.CourierId);
            this.HasRequired(t => t.LocationToObj)
                .WithMany(t => t.RequestsOfLocationTo)
                .HasForeignKey(d => d.LocationTo);
            this.HasRequired(t => t.LocationFromObj)
                .WithMany(t => t.RequestsOfLocationFrom)
                .HasForeignKey(d => d.LocationFrom);

        }
    }
}
