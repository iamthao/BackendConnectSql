using System;

namespace Framework.DomainModel.Entities.Common
{
    [Serializable]
    public class GoogleMapPoint
    {
        public double Lat { get; set; }
        public double Lng { get; set; }

        public GoogleMapPoint(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }

        public GoogleMapPoint()
        {
            // TODO: Complete member initialization
        }
    }
}