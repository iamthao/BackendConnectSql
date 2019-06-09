

namespace Framework.DomainModel.ValueObject
{
    public class LocationGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string StateOrProvinceOrRegion { get; set; }//
        public string CountryOrRegion { get; set; }//

        public string City { get; set; }
        public string Zip { get; set; }
        public string FullAddressSearch { get; set; }

        public string FullAddress
        {
            get { return Framework.Utility.CaculatorHelper.GetFullAddressCountry(Address1, Address2, City, StateOrProvinceOrRegion,Zip,CountryOrRegion); }
        }

        public byte[] AvailableTimeNoFormat { get; set; }
        
        public string AvailableTime {
            get
            {
                var result = "";
                if (AvailableTimeNoFormat != null && AvailableTimeNoFormat.Length == 7)
                {
                    for (int j = 0; j < AvailableTimeNoFormat.Length; j++)
                    {
                        if (AvailableTimeNoFormat[j] == 1)
                        {
                            switch (j)
                            {

                                case 0:
                                    result += "Monday, ";
                                    break;
                                case 1:
                                    result += "Tuesday, ";
                                    break;
                                case 2:
                                    result += "Wednesday, ";
                                    break;
                                case 3:
                                    result += "Thursday, ";
                                    break;
                                case 4:
                                    result += "Friday, ";
                                    break;
                                case 5:
                                    result += "Saturday, ";
                                    break;
                                case 6:
                                    result += "Sunday, ";
                                    break;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Remove(result.Length - 2, 2);
                }
                return result;
            }
        }

        public int? OpenHourNoFormat { get; set; }

        public string OpenHour
        {
            get
            {

                string s = "";
                if (OpenHourNoFormat != null)
                {
                    s = Framework.Utility.CaculatorHelper.ConvertIntToTime((int)OpenHourNoFormat);
                }
                return s;
            }
        }

        

        public int? CloseHourNoFormat { get; set; }

        public string CloseHour
        {
            get
            {
                
                string s = "";
                if (CloseHourNoFormat != null)
                {
                    s = Framework.Utility.CaculatorHelper.ConvertIntToTime((int)CloseHourNoFormat);
                }
                return s;
            }
        }

        public float? Lng { get; set; }
        public float? Lat { get; set; }

        
    }
    
}
