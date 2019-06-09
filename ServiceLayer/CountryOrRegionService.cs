using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer
{
    public class CountryOrRegionService : MasterFileService<CountryOrRegion>, ICountryOrRegionService
    {
        private readonly ICountryOrRegionRepository _countryOrRegionRepository;
        public CountryOrRegionService(ITenantPersistenceService tenantPersistenceService, ICountryOrRegionRepository countryOrRegionRepository,
            IBusinessRuleSet<CountryOrRegion> businessRuleSet = null)
            : base(countryOrRegionRepository, countryOrRegionRepository, tenantPersistenceService, businessRuleSet)
        {
            _countryOrRegionRepository = countryOrRegionRepository;
        }

        public List<LookupItemVo> GetAllCountryOrRegionForLookUp()
        {
            var countryorregion = _countryOrRegionRepository.GetAllCountryOrRegionForLookUp();
            return countryorregion;
        }
    }
}
