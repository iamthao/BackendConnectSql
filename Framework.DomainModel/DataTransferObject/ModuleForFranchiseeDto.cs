using System;
using System.Collections.Generic;
using Framework.DomainModel.Entities.Common;

namespace Framework.DomainModel.DataTransferObject
{
    public class ModuleForFranchiseeDto
    {
        public int? NumberOfCourier { get; set; }
        public List<int> ListModuleId { get; set; }
        public List<ModuleDocumentTypeOperationDto> ListModuleDocumentTypeOperations { get; set; }
    }

    public class ModuleDocumentTypeOperationDto
    {
        public int ModuleId { get; set; }
        public int DocumentTypeId { get; set; }
        public int OperationId { get; set; }
    }
}