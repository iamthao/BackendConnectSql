using Framework.DomainModel.Entities;

namespace ServiceLayer.Interfaces
{
    public interface IModuleService : IMasterFileService<Module>
    {
        dynamic GetModuleDocumentTypeOperationsGrid(int moduleId);
    }
}