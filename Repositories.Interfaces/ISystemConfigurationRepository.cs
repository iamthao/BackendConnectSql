﻿using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ISystemConfigurationRepository : IRepository<SystemConfiguration>, IQueryableRepository<SystemConfiguration>
    {
        List<SystemConfigurationGridVo> GetListSystemConfiguration();
    }
}
