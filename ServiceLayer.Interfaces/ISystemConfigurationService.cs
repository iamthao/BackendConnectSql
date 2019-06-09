using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ISystemConfigurationService : IMasterFileService<SystemConfiguration>
    {
        dynamic GetListSystemConfiguration();

    }
}
