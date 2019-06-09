using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{

    public interface IFranchiseeConfigurationRepository : IRepository<FranchiseeConfiguration>, IQueryableRepository<FranchiseeConfiguration>
    {
        FranchiseeConfiguration GetFranchiseeConfiguration();
        //
        int AddFranchiseeConfigurationBySqlString(FranchiseeConfiguration franchiseeConfiguration, string database);

        int DeleteAllFranchiseeConfigurationBySqlString( string database);
    }
}
