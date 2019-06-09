using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.Service.Diagnostics;
using Framework.Utility;
using ServiceLayer.Interfaces;

namespace QuickSpatchWindowsService.Job
{
    public class CourierStatusJob
    {
        public static void CourierStatusThread(object state)
        {
            var diagnosticService = Program.Container.Resolve<IDiagnosticService>();
            var courierService = Program.Container.Resolve<ICourierService>();
            var systemEventService = Program.Container.Resolve<ISystemEventService>();
            var timeout = 30;

            int.TryParse(XmlConfigReader.GetValue("CourierStatusTimeoutBySecond"), out timeout);

            try
            {
                var listCourier = courierService.ListAll();
                var listUpdate = new List<Courier>();

                //status online/ofline
                foreach (var courier in listCourier.Where(courier => (DateTime.UtcNow - courier.LastTime.GetValueOrDefault()).TotalSeconds >= timeout && courier.Status == (int)StatusCourier.Online))
                {
                    courier.Status = (int) StatusCourier.Offline;
                    listUpdate.Add(courier);
                    systemEventService.Add(EventMessage.CourierOffline, new Dictionary<EventMessageParam, string>()
                    {
                        {EventMessageParam.Courier, courier.User.LastName + " " + courier.User.FirstName + (string.IsNullOrEmpty(courier.User.MiddleName) ? "" : " " + courier.User.MiddleName)}
                    });
                }

                //reset request
                var current = DateTime.UtcNow;
                foreach (var courier in listCourier.Where(courier => courier.ServiceResetTime == null || (DateTime.UtcNow - courier.ServiceResetTime.GetValueOrDefault()).TotalSeconds >= 86400))
                {
                    courier.ServiceResetTime = current;
                    courier.CurrentReq = 0;
                    listUpdate.Add(courier);
                }

                courierService.UpdateListCourierForService(listUpdate);
            }
            catch (Exception ex)
            {
                diagnosticService.Error(ex);
            }
        }
    }
}
