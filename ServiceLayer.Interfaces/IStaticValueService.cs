
using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface IStaticValueService : IMasterFileService<StaticValue>
    {
        string GetNewRequestNumber();
        CheckSumChange CheckChangeTable();
    }
}