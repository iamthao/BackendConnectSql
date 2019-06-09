using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.Entities
{
    public class CountryOrRegion : Entity
    {
        public CountryOrRegion()
        {
            Locations = new List<Location>();
        }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
