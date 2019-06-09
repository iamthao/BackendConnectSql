namespace Framework.DomainModel.ValueObject
{
    public class LocationLookupItemVo :LookupItemVo
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}