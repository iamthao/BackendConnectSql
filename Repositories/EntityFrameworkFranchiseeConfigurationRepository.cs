using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ConfigValues;
using Database.Persistance.Tenants;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using System;
using System.Data.SqlClient;

namespace Repositories
{
    public class EntityFrameworkFranchiseeConfigurationRepository : EntityFrameworkTenantRepositoryBase<FranchiseeConfiguration>, IFranchiseeConfigurationRepository
    {
        public EntityFrameworkFranchiseeConfigurationRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {

        }

        public FranchiseeConfiguration GetFranchiseeConfiguration()
        {
            var query = GetAll().AsNoTracking();
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }

        public int AddFranchiseeConfigurationBySqlString(FranchiseeConfiguration franchiseeConfiguration, string database)
        {
            string insertSql = @"INSERT INTO dbo.[FranchiseeConfiguration]
                            ( FranchiseeId,
                              LicenseKey,
                              FranchiseeContact,
                              PrimaryContactPhone,
                              PrimaryContactEmail,
                              PrimaryContactFax,
                              PrimaryContactCellNumber,
                              Name,
                              City,
                              State,
                              Zip,
                              Address1,
                              Address2,
                              OfficePhone,
                              FaxNumber,
                              Logo,
                              IsQuickTour,
                              CreatedById ,
                              LastUserId ,
                              LastTime ,
                              CreatedOn  ,
                              IndustryId                          
                            )


                    VALUES  ( @FranchiseeId,
                              @LicenseKey,
                              @FranchiseeContact,
                              @PrimaryContactPhone,
                              @PrimaryContactEmail,
                              @PrimaryContactFax,
                              @PrimaryContactCellNumber,
                              @Name,
                              @City,
                              @State,
                              @Zip,
                              @Address1,
                              @Address2,
                              @OfficePhone,
                              @FaxNumber,    
                              @Logo, 
                              'true',
                              0,
                              0,
                              @LastTime,
                              @CreatedOn ,
                                @IndustryId
                            )";
            var parameterList = new List<SqlParameter>
            {
                new SqlParameter("@FranchiseeId", franchiseeConfiguration.FranchiseeId),
                new SqlParameter("@LicenseKey", franchiseeConfiguration.LicenseKey),
                new SqlParameter("@FranchiseeContact", franchiseeConfiguration.FranchiseeContact),
                new SqlParameter("@PrimaryContactPhone", franchiseeConfiguration.PrimaryContactPhone),
                new SqlParameter("@PrimaryContactEmail", franchiseeConfiguration.PrimaryContactEmail),
                new SqlParameter("@PrimaryContactFax", franchiseeConfiguration.PrimaryContactFax),
                new SqlParameter("@PrimaryContactCellNumber", franchiseeConfiguration.PrimaryContactCellNumber),
                new SqlParameter("@Name", franchiseeConfiguration.Name),
                new SqlParameter("@City", franchiseeConfiguration.City),
                new SqlParameter("@State", franchiseeConfiguration.State),
                new SqlParameter("@Zip", franchiseeConfiguration.Zip),
                new SqlParameter("@Address1", franchiseeConfiguration.Address1),
                new SqlParameter("@Address2", franchiseeConfiguration.Address2),
                new SqlParameter("@OfficePhone", franchiseeConfiguration.OfficePhone),
                new SqlParameter("@FaxNumber", franchiseeConfiguration.FaxNumber),
                new SqlParameter("@Logo", franchiseeConfiguration.Logo),
                new SqlParameter("@LastTime", DateTime.Now),
                new SqlParameter("@CreatedOn", DateTime.Now),
                new SqlParameter("@IndustryId", franchiseeConfiguration.IndustryId)
            };

            object[] parameters = parameterList.ToArray();
            insertSql = "use " + database + " " + insertSql;
            int result = TenantPersistenceService.CurrentWorkspace.Context.Database.ExecuteSqlCommand(insertSql, parameters);
            return result;
        }

        public int DeleteAllFranchiseeConfigurationBySqlString(string database)
        {
            string insertSql = @"DELETE FROM dbo.[FranchiseeConfiguration] ";
            insertSql = "use " + database + " " + insertSql;
            int result = TenantPersistenceService.CurrentWorkspace.Context.Database.ExecuteSqlCommand(insertSql);
            return result;
        }
    }
}