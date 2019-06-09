using System.Collections.Generic;

namespace Framework.DomainModel.DataTransferObject
{
    public class ListRequestDto : DtoBase
    {
        public List<RequestDto> RequestDtos { get; set; }

        public List<RequestHistoryDto> RequestHistoryDtos { get; set; }
    }
}