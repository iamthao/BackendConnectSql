using System.CodeDom.Compiler;
using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ILocationService : IMasterFileService<Location>
    {
        Location GetLocation();
        List<LocationDefaultVo> GetListLocation();

    }
}
