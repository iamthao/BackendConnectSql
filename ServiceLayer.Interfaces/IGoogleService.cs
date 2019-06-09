using System.Collections.Generic;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface IGoogleService
    {
        GoogleMapResultObject GetDirection(GoogleMapPoint origin, GoogleMapPoint destination);
        GoogleGetDistance GetDistance(GoogleMapPoint origin, GoogleMapPoint destination);
    }
}