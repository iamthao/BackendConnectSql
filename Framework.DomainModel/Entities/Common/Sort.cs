using System;

namespace Framework.DomainModel.Entities.Common
{
    [Serializable]
    public class Sort
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }
}