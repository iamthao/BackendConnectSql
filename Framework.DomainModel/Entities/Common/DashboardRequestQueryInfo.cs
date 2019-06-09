namespace Framework.DomainModel.Entities.Common
{
    public class DashboardRequestQueryInfo : QueryInfo
    {
        public int StatusId { get; set; }
        public int? CourierId { get; set; }
    }
}