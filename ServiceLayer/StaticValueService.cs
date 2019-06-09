using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class StaticValueService : MasterFileService<StaticValue>, IStaticValueService
    {
        private readonly IStaticValueRepository _staticValueRepository;
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;
        public StaticValueService(ITenantPersistenceService tenantPersistenceService, IStaticValueRepository staticValueRepository, 
            ISystemConfigurationRepository systemConfigurationRepository,
            IBusinessRuleSet<StaticValue> businessRuleSet = null)
            : base(staticValueRepository, staticValueRepository, tenantPersistenceService, businessRuleSet)
        {
            _staticValueRepository = staticValueRepository;
            _systemConfigurationRepository = systemConfigurationRepository;
        }

        public string GetNewRequestNumber()
        {
            var newRequestNumber = _staticValueRepository.GetNewRequestNumber();
            return ConverRequestNumberToValue(newRequestNumber);
        }

        private string ConverRequestNumberToValue(int requestNumber)
        {
            var requestNo = "REQ";
            var formatRequestNo =_systemConfigurationRepository.FirstOrDefault(o => o.SystemConfigType == SystemConfigType.RequestNo);
            if (formatRequestNo != null)
            {
                requestNo = formatRequestNo.Value;
            }
            if (requestNumber < 10000000000)
            {
                var result = string.Format( requestNo +"{0}", 10000000000 + requestNumber);
                return result.Replace(requestNo + "1", requestNo);
            }
            return string.Format(requestNo + "{0}", requestNumber);

        }
        private int ConverRequestValueToNumber(string requestNumber)
        {
            return int.Parse(requestNumber.Replace("REQ", ""));
        }

        public CheckSumChange CheckChangeTable()
        {
            return _staticValueRepository.CheckChangeTable();
        }
    }
}
