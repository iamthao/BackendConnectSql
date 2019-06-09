using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public class State : Entity
    {
        public State()
        {
        }

        public string Name { get; set; }
        public string AbbreviationName { get; set; }
    }
}
