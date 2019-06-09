using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.ValueObject;
using Framework.DomainModel.Entities;

namespace ServiceLayer.Interfaces
{
    public interface IGetLocationApiService 
    {
        GoogleGetLocation GetLocationApi(string zip, string country);
    }
}
