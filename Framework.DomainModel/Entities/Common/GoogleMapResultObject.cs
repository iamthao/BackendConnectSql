using System;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities.Common
{
    [Serializable]
    public class GoogleMapResultObject
    {
        public string Status { get; set; }
        public List<GoogleMapChildResultItem> routes { get; set; }

    }

    [Serializable]
    public class GoogleMapChildResultItem
    {
        public GoogleMapChildPoint overview_polyline { get; set; }
    }

    [Serializable]
    public class GoogleMapChildPoint
    {
        public string Points { get; set; }
    }
}