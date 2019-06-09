using System;
using System.Collections.Generic;
using System.Linq;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer
{
    public class SystemConfigurationService : MasterFileService<SystemConfiguration>, ISystemConfigurationService
    {
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;
        private readonly ILocationRepository _locationRepository;

        public SystemConfigurationService(ITenantPersistenceService tenantPersistenceService,
            ISystemConfigurationRepository systemConfigurationRepository, ILocationRepository locationRepository,
            IBusinessRuleSet<SystemConfiguration> businessRuleSet = null)
            : base(systemConfigurationRepository, systemConfigurationRepository, tenantPersistenceService, businessRuleSet)
        {
            _systemConfigurationRepository = systemConfigurationRepository;
            _locationRepository = locationRepository;
        }

        public dynamic GetListSystemConfiguration()
        {
            var list = _systemConfigurationRepository.GetListSystemConfiguration();
            foreach (var item in list)
            {
                if (item.SystemConfigType == SystemConfigType.DefaultLocationFrom && item.DataType == DataType.Int)
                {
                    var fromId = Convert.ToInt32(item.Value);
                    if (fromId > 0)
                    {
                        var from = _locationRepository.FirstOrDefault(o => o.Id == fromId);
                        if (from != null)
                        {
                            item.ValueNoFormat = from.Name;
                        }
                    }
                    else
                    {
                        item.ValueNoFormat = "Not Available";
                    }
                }
                else if (item.SystemConfigType == SystemConfigType.DefaultLocationTo && item.DataType == DataType.Int)
                {
                    var toId = Convert.ToInt32(item.Value);
                    if (toId > 0)
                    {
                        var to = _locationRepository.FirstOrDefault(o => o.Id == toId);
                        if (to != null)
                        {
                            item.ValueNoFormat = to.Name;
                        }
                    }
                    else
                    {
                        item.ValueNoFormat = "Not Available";
                    }
                }
                else
                {
                    item.ValueNoFormat = item.Value;
                }
            }
           
           return new { Data = list, TotalRowCount = list.Count() };
        }
    }
}
