using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Common;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class CourierService : MasterFileService<Courier>, ICourierService
    {
        private readonly ICourierRepository _courierRepository;
        private readonly ISystemEventRepository _systemEventRepository;
        private readonly IRequestRepository _requestRepository;
        public CourierService(ITenantPersistenceService tenantPersistenceService, ICourierRepository courierRepository, ISystemEventRepository systemEventRepository
            ,IRequestRepository requestRepository,IBusinessRuleSet<Courier> businessRuleSet = null)
            : base(courierRepository, courierRepository, tenantPersistenceService, businessRuleSet)
        {
            _courierRepository = courierRepository;
            _systemEventRepository = systemEventRepository;
            _requestRepository = requestRepository;
        }

        public Courier GetCourierInfo(int id)
        {
            return _courierRepository.GetCourierInfo(id);
        }

        public Courier GetCourierWithUsernameAndPassword(UsernameAndPasswordDto userInfo)
        {
            var hashedPassword = PasswordHelper.HashString(userInfo.Password, userInfo.Username);
            var courier =
                _courierRepository.FirstOrDefault(
                    o => o.User.UserName == userInfo.Username && o.User.Password == hashedPassword);
            if (courier != null)
            {
                if (String.IsNullOrEmpty(courier.Imei) || courier.Imei.Equals(userInfo.Imei))
                {
                    courier.Imei = userInfo.Imei;
                    courier.Status = (int)StatusCourier.Online;
                    _courierRepository.Update(courier);
                    //add system event
                    _systemEventRepository.Add(EventMessage.CourierLogin, new Dictionary<EventMessageParam, string> { { EventMessageParam.Courier, courier.User.FullName} });
                    _courierRepository.Commit();
                }
            }
            return courier;
        }

        public dynamic GetListCourierForDashboard(QueryInfo queryInfo)
        {
            return _courierRepository.GetListCourierForDashboard(queryInfo);
        }

        public dynamic GetCouriersForSchedule(QueryInfo queryInfo)
        {

            return _courierRepository.GetCouriersForSchedule(queryInfo);
        }

        public dynamic GetAutoAssignCourier()
        {
            return _courierRepository.GetAutoAssignCourier();
        }

        public List<LookupItemVo> GetLookupForTracking(LookupQuery query, Func<Courier, LookupItemVo> selector)
        {
            return _courierRepository.GetLookupForTracking(query, selector);
        }

        public int CheckConnection(CheckConnectDto checkConnectDto)
        {
            var courier = _courierRepository.FirstOrDefault(o => o.Id == checkConnectDto.Id);
            if (courier != null)
            {
                if (courier.Imei == checkConnectDto.Imei)
                {
                    if (courier.Status == (int) StatusCourier.Offline)
                    {
                        //add system event
                        _systemEventRepository.Add(EventMessage.CourierOnline, new Dictionary<EventMessageParam, string> { { EventMessageParam.Courier, courier.User.FullName } });
                    }
                    var currentReq = _requestRepository.GetById(checkConnectDto.CurrentRequest);
                    
                    if (currentReq != null && currentReq.Status == (int) StatusRequest.Started)
                    {
                        courier.CurrentReq = checkConnectDto.CurrentRequest;   
                    }
                    
                    courier.Status = (int)StatusCourier.Online;
                    courier.CurrentVelocity = checkConnectDto.CurrentVelocity;
                    courier.CurrentLng = checkConnectDto.CurrentLng;
                    courier.CurrentLat = checkConnectDto.CurrentLat;
                    _courierRepository.Update(courier);
                    _courierRepository.Commit();
                    return 1;
                }
                return 2;
            }
            return 0;
        }

        public void LogOut(int courierId)
        {
            var courier = _courierRepository.GetById(courierId);


            if (courier != null)
            {
                courier.Imei = "";
                courier.Status = (int)StatusCourier.Offline;
                _courierRepository.Update(courier);
                //add system event
                _systemEventRepository.Add(EventMessage.CourierLogout, new Dictionary<EventMessageParam, string> { { EventMessageParam.Courier, courier.User.FullName } });
                _courierRepository.Commit();
            }
        }

        public void UpdateListCourierForService(IList<Courier> listCourier)
        {
            foreach (var courier in listCourier)
            {
                Repository.Update(courier);
            }
            Repository.Commit();
        }

        public dynamic GetPositionCurrentOfCourier(int courierId)
        {
            return _courierRepository.GetPositionCurrentOfCourier(courierId);
        }

        public dynamic GetAllCourierOnlineLocation()
        {
            return _courierRepository.GetAllCourierOnlineLocation();
        }


        public Courier UpdateWithouCheckBussinessRule(Courier courier)
        {
            _courierRepository.Update(courier);
            _courierRepository.Commit();
            return courier;
        }

        public List<LookupItemVo> GetLookupForReport(LookupQuery query, Func<Courier, LookupItemVo> selector)
        {
            return _courierRepository.GetLookupForReport(query, selector);
        }
    }
}