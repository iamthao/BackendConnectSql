using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class CourierScheduleGridVo : ReadOnlyGridVo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Name
        {
            get { return Framework.Utility.CaculatorHelper.GetFullName(FirstName, MiddleName, LastName); }
        }
        public int RouteNum { get; set; }
    }
}
