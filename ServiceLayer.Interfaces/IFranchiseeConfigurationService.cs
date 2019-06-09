using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.Entities;

namespace ServiceLayer.Interfaces
{
    public interface IFranchiseeConfigurationService : IMasterFileService<FranchiseeConfiguration>
    {
        FranchiseeConfiguration GetFranchiseeConfiguration();

        //
        void ValidateBusinessRulesFranchiseeConfig(FranchiseeConfiguration franchiseeConfiguration);
    }
}
