using System;

namespace Framework.DomainModel.Interfaces
{
    public interface IEntity : ICloneable
    {
        int Id { get; set; }
    }
}
