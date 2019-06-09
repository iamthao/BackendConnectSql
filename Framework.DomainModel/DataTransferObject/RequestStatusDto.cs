using System.Collections.Generic;

namespace Framework.DomainModel.DataTransferObject
{
    public class RequestStatusDto : DtoBase
    {
        public List<UpdateRequestDto> UpdateRequestDtos { get; set; } 
    }
}