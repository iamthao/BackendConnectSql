namespace Framework.DomainModel.DataTransferObject
{
    public class LocationDto : DtoBase
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }

        public string FullAddress
        {
            get { return Framework.Utility.CaculatorHelper.GetFullAddress(Address1, Address2, City, State, Zip); }
        }

        public string Zip { get; set; }
        public string State { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

    }
}