using System.Collections.Generic;
using Framework.DomainModel.Entities;

namespace ServiceLayer.Interfaces
{
    public interface IRequestHistoryService : IMasterFileService<RequestHistory>
    {
        void AddListRequestHistoryForWindowsService(List<RequestHistory> listRequestHistories);
    }
}
