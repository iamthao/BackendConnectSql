using System.Collections.Generic;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class LocationService : MasterFileService<Location>, ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        

        public LocationService(ITenantPersistenceService tenantPersistenceService,
                              ILocationRepository locationRepository,
                            IBusinessRuleSet<Location> businessRuleSet = null)
                : base(locationRepository, locationRepository,tenantPersistenceService,businessRuleSet)
        {
            _locationRepository = locationRepository;
     
        }

        public Location GetLocation()
        {
            var location = _locationRepository.GetLocation();
            return location;
        }

        public List<LocationDefaultVo> GetListLocation()
        {
            var location = _locationRepository.GetListLocation();
            return location;
        }

    }
}
