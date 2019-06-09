using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.Service.Diagnostics;
using Framework.Utility;
using ServiceLayer.Interfaces;

namespace QuickSpatchWindowsService.Job
{
    public class RequestStatusJob
    {
        public static void RequestStatusThread(object state)
        {
            var diagnosticService = Program.Container.Resolve<IDiagnosticService>();
            var systemEventService = Program.Container.Resolve<ISystemEventService>();

            var listRequestUpdate = new List<Request>();
            var timeout = 600;
            int.TryParse(XmlConfigReader.GetValue("RequestStatusTimeoutBySecond"), out timeout);

            try
            {
                var requestService = Program.Container.Resolve<IRequestService>();
                var requestHistoryService = Program.Container.Resolve<IRequestHistoryService>();

                var listRequest = requestService.Get(p => p.Status == (int)StatusRequest.Waiting || p.Status == (int)StatusRequest.Sent);
                foreach (var request in listRequest.Where(request => (DateTime.UtcNow - request.SendingTime.GetValueOrDefault()).TotalSeconds >= timeout))
                {
                    request.Status = (int)StatusRequest.Abandoned;
                    listRequestUpdate.Add(request);
                    systemEventService.Add(EventMessage.RequestAbandoned, new Dictionary<EventMessageParam, string>()
                    {
                        {EventMessageParam.Request, request.RequestNo},
                        {EventMessageParam.Courier, request.Courier.User.LastName + " " + request.Courier.User.FirstName + (string.IsNullOrEmpty(request.Courier.User.MiddleName) ? "" : " " + request.Courier.User.MiddleName)}
                    });
                }

                var listRequestNotSend = requestService.Get(p => p.Status == (int)StatusRequest.NotSent);
                foreach (var request in listRequestNotSend.Where(request => (DateTime.UtcNow - request.SendingTime.GetValueOrDefault()).TotalSeconds >= 0))
                {
                    request.Status = (int)StatusRequest.Sent;
                    listRequestUpdate.Add(request);
                    systemEventService.Add(EventMessage.RequestSent, new Dictionary<EventMessageParam, string>()
                    {
                        {EventMessageParam.Request, request.RequestNo},
                        {EventMessageParam.Courier, request.Courier.User.LastName + " " + request.Courier.User.FirstName + (string.IsNullOrEmpty(request.Courier.User.MiddleName) ? "" : " " + request.Courier.User.MiddleName)}
                    });
                }

                
                var history = new List<RequestHistory>();
                var listRequestExpired = requestService.Get(p => p.Status != (int)StatusRequest.Cancelled && p.Status != (int)StatusRequest.Completed && !(p.IsExpired ?? false));
                foreach (var request in listRequestExpired.Where(request => (DateTime.UtcNow - request.CreatedOn.GetValueOrDefault()).TotalSeconds >= request.ExpiredTime.GetValueOrDefault(0)))
                {
                    request.IsExpired = true;
                    if (request.Courier.CurrentReq == request.Id)
                    {
                        request.Courier.CurrentReq = 0;
                    }
                    listRequestUpdate.Add(request);
                    //add to history
                    history.Add(new RequestHistory()
                    {
                        ActionType = (int)HistoryRequestActionType.Expired,
                        Comment = "Request expired",
                        CourierId = request.CourierId.GetValueOrDefault(),
                        LastRequestStatus = request.Status,
                        TimeChanged = DateTime.UtcNow,
                        RequestId = request.Id
                    });
                    systemEventService.Add(EventMessage.RequestExpired, new Dictionary<EventMessageParam, string>()
                    {
                        {EventMessageParam.Request, request.RequestNo},
                        {EventMessageParam.Courier, request.Courier.User.LastName + " " + request.Courier.User.FirstName + (string.IsNullOrEmpty(request.Courier.User.MiddleName) ? "" : " " + request.Courier.User.MiddleName)}
                    });
                }

                requestService.UpdateListRequestForService(listRequestUpdate);
                requestHistoryService.AddListRequestHistoryForWindowsService(history);
            }
            catch (Exception ex)
            {
                diagnosticService.Error(ex);
            }
        }
    }
}