namespace Framework.DomainModel.DataTransferObject
{
    public class UsernameAndPasswordDto : DtoBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Imei { get; set; }
    }
}