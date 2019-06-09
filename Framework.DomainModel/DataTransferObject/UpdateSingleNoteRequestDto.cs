namespace Framework.DomainModel.DataTransferObject
{
    public class UpdateSingleNoteRequestDto : DtoBase
    {
        public int RequestId { get; set; }
        public NoteDto NoteDto { get; set; }
        public int CourierId { get; set; }
    }
}