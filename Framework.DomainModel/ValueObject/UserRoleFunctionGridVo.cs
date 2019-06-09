namespace Framework.DomainModel.ValueObject
{
    public class UserRoleFunctionGridVo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsView { get; set; }
        public bool IsInsert { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsProcess { get; set; }
    }
}