using System;
using Framework.DomainModel.Common;

namespace Framework.DomainModel.ValueObject
{
    public class DashboardRequestChartVo : ReadOnlyGridVo
    {
        public string Category
        {
            get
            {
                string result;
                switch (Id)
                {
                    case (int)StatusRequest.NotSent:
                        result = "Not Sent";
                        break;
                    default:
                        result = Enum.GetName(typeof (StatusRequest), Id);
                        break;
                }

                return result;
            }
        }

        public int Value { get; set; }

        public string Color
        {
            get
            {
                var result = "#cccccc";
                switch (Id)
                {
                    case (int)StatusRequest.Abandoned:
                        result = "#5f5f5f";
                        break;

                    case (int)StatusRequest.Cancelled:
                        result = "#d9534f";
                        break;

                    case (int)StatusRequest.Completed:
                        result = "#5cb85c";
                        break;

                    case (int)StatusRequest.Declined:
                        result = "#000000";
                        break;

                    case (int)StatusRequest.NotSent:
                        result = "#cccccc";
                        break;

                    case (int)StatusRequest.Received:
                        result = "#00ced1";
                        break;

                    case (int)StatusRequest.Sent:
                        result = "#daa520";
                        break;

                    case (int)StatusRequest.Started:
                        result = "#00bfff";
                        break;

                    case (int)StatusRequest.Waiting:
                        result = "#ff8c00";
                        break;

                    default:
                        break;
                }

                return result;
            }
        }

        public bool Selected
        {
            get
            {
                if (Id == (int) StatusRequest.Completed)
                {
                    return true;
                }
                return false;
            }
        }
    }
}