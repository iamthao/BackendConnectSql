namespace Framework.DomainModel.DataTransferObject
{
    public class CheckConnectDto : DtoBase
    {
        public string Imei { get; set; }
        public int CurrentRequest { get; set; }
        public double? CurrentLat { get; set; }
        public double? CurrentLng { get; set; }
        public double? CurrentVelocity { get; set; }
    }
}