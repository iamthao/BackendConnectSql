using System;
using System.Collections.Generic;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{

    public class CourierOnlineVo
    {
        public int CourierId { get; set; }
        public int? CurrentRequestId{ get; set; }
        public string CurrentRequestNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public byte[] Avatar { get; set; }
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

        //private double? _currenVelocity;
        public double? CurrentVelocity
        {
            get
            {
                if (CurrentVelocityNoConvert.GetValueOrDefault() < 0)
                {
                    return 0;
                }
                return CurrentVelocityNoConvert.GetValueOrDefault().MetersPerSecondTpMph();
            }
            
        }

        public double? CurrentVelocityNoConvert { get; set; }
    }
}