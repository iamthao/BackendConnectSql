namespace Framework.DomainModel.ValueObject
{
    public class Tenant
    {
        public string Name { get; set; }

        public string Server { get; set; }

        public string Database { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}