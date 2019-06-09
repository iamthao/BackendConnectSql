using System.Collections.Generic;
using System.Net;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Newtonsoft.Json;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class GoogleService : IGoogleService
    {
        public GoogleMapResultObject GetDirection(GoogleMapPoint origin, GoogleMapPoint destination)
        {
            var originString = string.Format("{0},{1}", origin.Lat, origin.Lng);
            var destinationString = string.Format("{0},{1}", destination.Lat, destination.Lng);
            
            var requestUrl = string.Format("http://maps.google.com/maps/api/directions/json?origin={0}&destination={1}&sensor=false", originString, destinationString);

            var client = new WebClient();
            var result = client.DownloadString(requestUrl);
            return JsonConvert.DeserializeObject<GoogleMapResultObject>(result);
        }

        public GoogleGetDistance GetDistance(GoogleMapPoint origin, GoogleMapPoint destination)
        {
            var originString = string.Format("{0},{1}", origin.Lat, origin.Lng);
            var destinationString = string.Format("{0},{1}", destination.Lat, destination.Lng);

            var requestUrl = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?origins={0}&destinations={1}", originString, destinationString);
            var client = new WebClient();
            var result = client.DownloadString(requestUrl);
            return JsonConvert.DeserializeObject<GoogleGetDistance>(result);
        }
    }
}