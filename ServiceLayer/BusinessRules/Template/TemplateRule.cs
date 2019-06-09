using System;
using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BusinessRules.Template
{
    public class TemplateRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly ITemplateRepository _templateRepository;

        public TemplateRule(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }
        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var template = instance as Framework.DomainModel.Entities.Template;
            var validationResult = new List<ValidationResult>();
            if (template != null)
            {
                if (String.IsNullOrEmpty(template.Title))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Title");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (template.TemplateType == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Template Type");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (String.IsNullOrEmpty(template.Content))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Content");
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
            get { return "TemplateRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}