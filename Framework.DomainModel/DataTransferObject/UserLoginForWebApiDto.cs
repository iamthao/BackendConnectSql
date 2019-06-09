namespace Framework.DomainModel.DataTransferObject
{
    public class UserLoginForWebApiDto : DtoBase
    {
        
    }

    public class TokenStoreDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    
}