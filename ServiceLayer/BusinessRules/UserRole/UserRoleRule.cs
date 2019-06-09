using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BusinessRules.UserRole
{
    public class UserRoleRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleRule(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public BusinessRuleResult Execute(IEntity instance)
        {
            bool failed = false;
            var userRole = instance as Framework.DomainModel.Entities.UserRole;
            var validationResult = new List<ValidationResult>();
            if (userRole != null)
            {
                if (string.IsNullOrEmpty(userRole.Name))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (!string.IsNullOrEmpty(userRole.Name) && _userRoleRepository.CheckExist(o => o.Name.Trim().ToLower() == userRole.Name.Trim().ToLower() && o.Id != userRole.Id))
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
            get { return "UserRoleRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}