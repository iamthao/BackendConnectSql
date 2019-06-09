using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.ValueObject
{
    public class LocationDefaultVo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string StateOrProvinceOrRegion { get; set; }//
        public string CountryOrRegion { get; set; }//

        public string City { get; set; }
        public string Zip { get; set; }

        public string FullAddress
        {
            get { return Utility.CaculatorHelper.GetFullAddressCountry(Address1, Address2, City, StateOrProvinceOrRegion, Zip, CountryOrRegion); }
        }
    }

}
