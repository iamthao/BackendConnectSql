using Framework.DataAnnotations;

namespace Framework.DomainModel.Entities
{
    public class Customer : Entity
    {
        [LocalizeRequired(FieldName = "Name")]
        public string Name { get; set; }
    }
}