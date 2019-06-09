using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public class Contact : Entity
    {
        public Contact()
        {
        }

        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
