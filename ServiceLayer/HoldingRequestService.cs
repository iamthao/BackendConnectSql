using System;
using System.Collections.Generic;
using System.Transactions;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class HoldingRequestService : MasterFileService<HoldingRequest>, IHoldingRequestService
    {
        private readonly IHoldingRequestRepository _holdingRequestRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly ISystemEventRepository _systemEventRepository;
        private readonly IStaticValueService _staticValueService;
        private readonly ICourierRepository _courierRepository;
        private readonly IRequestService _requestService;

        public HoldingRequestService(ITenantPersistenceService tenantPersistenceService,
            IHoldingRequestRepository holdingRequestRepository,
            IRequestRepository requestRepository,
            IStaticValueService staticValueService,
            ISystemEventRepository systemEventRepository,
            ICourierRepository courierRepository,
            IRequestService requestService,
            IBusinessRuleSet<HoldingRequest> businessRuleSet = null)
            : base(holdingRequestRepository, holdingRequestRepository, tenantPersistenceService, businessRuleSet)
        {
            FriendlyEntityName = "Holding request";
            _holdingRequestRepository = holdingRequestRepository;
            _requestRepository = requestRepository;
            _staticValueService = staticValueService;
            _systemEventRepository = systemEventRepository;
            _courierRepository = courierRepository;
            _requestService = requestService;
        }

        public void SendHoldingRequest(SendHoldingRequestItemVo holdingRequestItem)
        {
            using (var tran = new TransactionScope())
            {
                var holdingRequest = GetById(holdingRequestItem.HoldingRequestSelectedId);

                var request = new Request()
                {
                    RequestNo = _staticValueService.GetNewRequestNumber(),
                    LocationFrom = holdingRequest.LocationFrom,
                    LocationTo = holdingRequest.LocationTo,
                    StartTime = holdingRequest.StartTime.GetValueOrDefault(),
                    EndTime = holdingRequest.EndTime.GetValueOrDefault(),
                    SendingTime = holdingRequestItem.IsStat ? DateTime.UtcNow : holdingRequestItem.SendingTime,
                    IsStat = holdingRequestItem.IsStat,
                    Status = (int)StatusRequest.NotSent,
                    CourierId = holdingRequestItem.CourierId,
                    Description = holdingRequest.Description,
                    ExpiredTime = holdingRequestItem.ExpiredTime,
                    SaveSystemEvent = false
                };


                _requestService.Add(request);

                _holdingRequestRepository.Delete(holdingRequest);
                _holdingRequestRepository.Commit();


                _systemEventRepository.Add(EventMessage.RequestCreated, new Dictionary<EventMessageParam, string>()
                    {
                        { EventMessageParam.Request, request.RequestNo},
                        { EventMessageParam.Courier, _courierRepository.GetById(holdingRequestItem.CourierId.GetValueOrDefault()).User.UserName }
                    });



                tran.Complete();
            }
        }


        public void SendListHoldingRequest(SendListHoldingRequestItemVo listHoldingRequestItem)
        {
            using (var tran = new TransactionScope())
            {
                foreach (var holdingRequestSelectedId in listHoldingRequestItem.HoldingRequestSelectedIds)
                {
                    var holdingRequest = GetById(holdingRequestSelectedId);

                    var request = new Request()
                    {
                        RequestNo = _staticValueService.GetNewRequestNumber(),
                        LocationFrom = holdingRequest.LocationFrom,
                        LocationTo = holdingRequest.LocationTo,
                        StartTime = holdingRequest.StartTime.GetValueOrDefault(),
                        EndTime = holdingRequest.EndTime.GetValueOrDefault(),
                        SendingTime = listHoldingRequestItem.SendingTime,
                        IsStat = listHoldingRequestItem.IsStat,
                        Status = (int)StatusRequest.NotSent,
                        CourierId = listHoldingRequestItem.CourierId,
                        Description = holdingRequest.Description,
                        ExpiredTime = listHoldingRequestItem.ExpiredTime
                    };

                    _requestService.Add(request);

                    _holdingRequestRepository.Delete(holdingRequest);

                    _systemEventRepository.Add(EventMessage.RequestSent, new Dictionary<EventMessageParam, string>()
                    {
                        {EventMessageParam.Request, request.RequestNo},
                        {EventMessageParam.Courier, _courierRepository.GetById(listHoldingRequestItem.CourierId.GetValueOrDefault()).User.UserName}
                    });
                }

                _holdingRequestRepository.Commit();

                tran.Complete();
            }
        }
    }
}