using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.XPath;
using Database.Persistance.Tenants;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkSystemEventRepository : EntityFrameworkTenantRepositoryBase<SystemEvent>, ISystemEventRepository
    {
        private readonly IFranchiseeConfigurationRepository _franchiseeConfigurationRepository;

        public EntityFrameworkSystemEventRepository(ITenantPersistenceService persistenceService, IFranchiseeConfigurationRepository franchiseeConfigurationRepository)
            : base(persistenceService)
        {
            _franchiseeConfigurationRepository = franchiseeConfigurationRepository;
        }

        public List<SystemEventGridVo> GetEventsDashboard()
        {
            var config = _franchiseeConfigurationRepository.FirstOrDefault();
            var title = config.IndustryId == null ? "courier" : config.Industry.DisplayLabel;

            var query = GetAll().OrderByDescending(o => o.CreatedOn).Skip(0).Take(50).Select(o => new SystemEventGridVo
            {
                Id = o.Id,
                DescriptionOld = o.Description,
                CreatedOnDateTime = o.CreatedOn,
                Title = title
            });

            return query.ToList();
        }

        public void Add(EventMessage eventMessage, Dictionary<EventMessageParam, string> dictionParam)
        {
            var config = _franchiseeConfigurationRepository.FirstOrDefault();
            var title = config.IndustryId == null ? "courier" : config.Industry.DisplayLabel;

            var mess = XmlDataHelpper.Instance.GetMessageValue(XmlDataTypeEnum.EventMessage.ToString(),
                ((int)eventMessage).ToString(CultureInfo.InvariantCulture),
                dictionParam.ToDictionary(s => s.Key.ToString(), s => s.Value), title);

            var systemEvent = new SystemEvent
            {
                Description = mess,
                EventType = (int)eventMessage
            };

            base.Add(systemEvent);
            base.Commit();
        }
        public dynamic GetNotifyDecline(QueryInfo queryInfo)
        {
            var notifyDeclineQueryInfo = (NotifyDeclineQueryInfo)queryInfo;
            notifyDeclineQueryInfo.Time = notifyDeclineQueryInfo.Time.ToUniversalTime();
            var query =
                GetAll()
                    .Where(
                        o =>
                            o.CreatedOn >= notifyDeclineQueryInfo.Time &&
                            o.EventType == (int)EventMessage.CourierDeclinedRequest)
                    .Select(o => new NotifyDeclineGridVo
                    {
                        Event = o.Description,
                        CreatedDate = o.CreatedOn
                    })
                    .OrderByDescending(o => o.CreatedDate);
            if (query.Any())
            {
                queryInfo.TotalRecords = query.Count();
                var data = query.Skip(queryInfo.Skip)
                .Take(queryInfo.Take).ToList().ToList();
                return new { Data = data, TotalRowCount = queryInfo.TotalRecords };
            }
            return null;

          
        }
    }
}
