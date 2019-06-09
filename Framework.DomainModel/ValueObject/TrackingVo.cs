using System;
using System.Collections.Generic;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class TrackingVo
    {

        public int RequestId { get; set; }
        public string RequestNo { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public byte[] Avatar { get; set; }
        public bool IsActiveRequest { get; set; }
        public int? CourierId { get; set; }
        public string AvatarImage {
            get
            {
                if (Avatar != null)
                {
                    var image=Utility.ResizeImageService.RoundCorners(Avatar,225);
                    return "data:image/jpg;base64," + Convert.ToBase64String(image);

                }
                return null;
            }
        }
        public string FullName {

            get { return Framework.Utility.CaculatorHelper.GetFullName(FirstName, MiddleName, LastName); }
        }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public double Direction { get; set; }
        public double? Distance { get; set; }
        public string TimeTracking { get; set; }
        public bool IsFinish { get; set; }

        private double? _velocity;

        public double? Velocity
        {
            get
            {
                return _velocity.MetersToMiles();
            }
            set
            {
                _velocity = value;
            }
        }


        public string IconDirectionName
        {
            get
            {
                return Math.Round(Direction / 10, 0) * 10 + ".png";
            }
        }

        public List<GoogleMapApiHelper.LocationVo> LocationList
        {
            get
            {
                return Address.DecodePolylinePoints();
            }
        }
    }
}