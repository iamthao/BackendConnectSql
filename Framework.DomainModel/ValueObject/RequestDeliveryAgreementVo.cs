namespace Framework.DomainModel.ValueObject
{
    public class RequestDeliveryAgreementVo : ReadOnlyGridVo
    {
        public string RequestNo { get; set; }
        public string RequestFrom { get; set; }
        public string RequestTo { get; set; }
        public int RequestTimes { get; set; }
        public string RequestDistance { get; set; }
        public bool RequestAgreed { get; set; }
        public string Signature { get; set; }
        public string RequestFromName { get; set; }
        public string RequestToName { get; set; }
        public bool? IsAgreed { get; set; }
    }
}