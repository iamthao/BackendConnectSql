using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BusinessRules.Module
{
    public class ModuleRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleRule(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }
        
        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var module = instance as Framework.DomainModel.Entities.Module;
            var validationResult = new List<ValidationResult>();
            if (module != null)
            {
                if (string.IsNullOrEmpty(module.Name))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (!string.IsNullOrEmpty(module.Name) && _moduleRepository.CheckExist(o => o.Name == module.Name && o.Id != module.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();
        }

        public string Name
        {
            get { return "ModuleRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}