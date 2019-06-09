using System;
using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;

namespace Framework.Exceptions
{
    /// <summary>
    ///     Base of business logic exception
    /// </summary>
    [Serializable]
    // Summary:
    //     Wrapper exception for exceptions thrown because one or more business rules
    //     have failed.
    //
    // Remarks:
    //     This specific exception type allows to differenciate between exceptions thrown
    //     by business rules, and exceptions where things just go wrong. If propagated
    //     through a web service, this exception gives a more detailed explaination
    //     why the operation has failed.  
    public class BusinessRuleException : UserVisibleException
    {
        private readonly ICollection<BusinessRuleResult> m_FailedRules;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleException"/> class.
        /// </summary>
        /// <param name="messageResourceKey"></param>
        /// <param name="failedRules">A set of <see cref="BusinessRuleResult"/>
        /// to transport information what has been evaluated as a business rule
        /// violation, and on which object.</param>
        public BusinessRuleException(string messageResourceKey, BusinessRuleResult[] failedRules)
            : base(messageResourceKey)
        {
            if (failedRules == null)
            {
                m_FailedRules = new BusinessRuleResult[0];
            }
            else
            {
                m_FailedRules = failedRules;
            }
        }

        /// <summary>
        /// Gets a collection of business rule result that have causes the exception to be 
        /// thrown. 
        /// </summary>       
        /// <value>A set of <see cref="BusinessRuleResult"/>
        /// to transport information what has been evaluated as a business rule
        /// violation, and on which object.</value>
        public ICollection<BusinessRuleResult> FailedRules
        {
            get
            {
                return m_FailedRules;
            }
        }
    }
}