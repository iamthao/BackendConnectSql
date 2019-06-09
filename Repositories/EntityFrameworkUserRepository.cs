using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;

namespace Repositories
{
    public class EntityFrameworkUserRepository : EntityFrameworkTenantRepositoryBase<User>, IUserRepository
    {
        public EntityFrameworkUserRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("UserName");
            SearchColumns.Add("Role");
            SearchColumns.Add("FullNameSearch");
            SearchColumns.Add("HomePhone");
            SearchColumns.Add("MobilePhone");
            SearchColumns.Add("Email");
            DisplayColumnForCombobox = "FirstName";
            Includes.Add("UserRole");
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryUser = queryInfo as UserQueryInfo;
            var queryResult = (from entity in GetAll()
                               where entity.UserRole.Name != "Courier" && entity.Id != queryUser.CurrentUserId                               
                               select new { entity }).Select(s => new UserGridVo
                               {
                                   Id = s.entity.Id,
                                   UserName = String.IsNullOrEmpty(s.entity.UserName) ? "" : s.entity.UserName,
                                   Password = s.entity.Password,
                                   FirstName = String.IsNullOrEmpty(s.entity.FirstName) ? "" : s.entity.FirstName,
                                   MiddleName = String.IsNullOrEmpty(s.entity.MiddleName) ? "" : s.entity.MiddleName,
                                   LastName = String.IsNullOrEmpty(s.entity.LastName) ? "" : s.entity.LastName,
                                   FullNameSearch = s.entity.LastName + " " + s.entity.FirstName + " " + s.entity.MiddleName,
                                   Role = s.entity.UserRole != null ? s.entity.UserRole.Name : "",
                                   Email = String.IsNullOrEmpty(s.entity.Email) ? "" : s.entity.Email,
                                   HomePhone = String.IsNullOrEmpty(s.entity.HomePhone) ? "" : s.entity.HomePhone,
                                   MobilePhone = String.IsNullOrEmpty(s.entity.MobilePhone) ? "" : s.entity.MobilePhone,
                                   IsActive = s.entity.IsActive,
                                   Avatar =  s.entity.Avatar,
                                   UserRoleId = s.entity.UserRoleId >=0 ? s.entity.UserRoleId:null,
                                   //IsQuickspatchUser = s.entity.IsQuickspatchUser==true ? true:false
                               }).OrderBy(queryInfo.SortString );
            var test = queryResult;
            return queryResult;

           
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "FirstName", Dir = "" } };
            }
            queryInfo.Sort.ForEach(x =>
            {
                if (x.Field == "FullName")
                {
                    x.Field = "FirstName";
                }
                else if (x.Field == "HomePhoneInFormat")
                {
                    x.Field = "HomePhone";
                }
                else if (x.Field == "MobilePhoneInFormat")
                {
                    x.Field = "MobilePhone";
                }
                else
                {
                    x.Field = string.Format("{0}", x.Field);
                }
            });
        }

        protected override string BuildLookupCondition(LookupQuery query)
        {
            var where = new StringBuilder();
            @where.Append("(");
            var innerWhere = new List<string>();
            var queryDisplayName = String.Format("FirstName.Contains(\"{0}\") OR LastName.Contains(\"{0}\") OR MiddleName.Contains(\"{0}\")", query.Query);
            innerWhere.Add(queryDisplayName);
            @where.Append(String.Join(" OR ", innerWhere.ToArray()));
            @where.Append(")");
            if (query.HierachyItems != null)
            {
                foreach (var parentItem in query.HierachyItems.Where(parentItem => parentItem.Value != string.Empty && parentItem.Value != "-1"
                                                                                    && parentItem.Value != "0" && !parentItem.IgnoredFilter))
                {
                    var filterValue = parentItem.Value.Replace(",", string.Format(" OR {0} = ", parentItem.Name));
                    @where.Append(string.Format(" AND ( {0} = {1})", parentItem.Name, filterValue));
                }
            }
            return @where.ToString();
        }

        public User GetUserByUserNameAndPass(string username, string password)
        {
            return GetAll().SingleOrDefault(o => o.UserName == username && o.Password == password && o.IsActive);
        }

        public User GetUserFromHashStringPasswordAndUsername(string code)
        {
            var listUsers = GetAll();
            if (listUsers != null && listUsers.Any())
            {
                foreach (var o in listUsers)
                {
                    var hashCode = PasswordHelper.HashString(o.Id.ToString(), o.UserName);
                    if (hashCode == code)
                    {
                        return o;
                    }

                }
            }
            return null;
            
        }

        public int AddUserBySqlString(User user, string database)
        {
            string insertSql = @"INSERT INTO dbo.[User]
                            ( UserName ,
                              Password ,
                              UserRoleId ,
                              IsActive ,
                              FirstName ,
                              LastName ,
                              HomePhone ,
                              MobilePhone ,
                              Email ,
                              
                              CreatedById ,
                              LastUserId ,
                              LastTime ,
                              CreatedOn,
                              IsQuickspatchUser
                            )

                    VALUES  ( @UserName,
                              @Password,
                              @UserRoleId,
                              @IsActive,
                              @FirstName,
                              @LastName,
                              @HomePhone,
                              @MobilePhone,
                              @Email,
                              0,
                              0,
                              @LastTime,
                              @CreatedOn,
                              @IsQuickspatchUser
                            )";
            var parameterList = new List<SqlParameter>
            {
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@UserRoleId", user.UserRoleId),
                new SqlParameter("@IsActive", user.IsActive),
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@HomePhone", user.HomePhone),
                new SqlParameter("@MobilePhone", user.MobilePhone),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@LastTime", DateTime.Now),
                new SqlParameter("@CreatedOn", DateTime.Now),
                new SqlParameter("@IsQuickspatchUser", true),
                
            };
            object[] parameters = parameterList.ToArray();
            insertSql = "use " + database + " " + insertSql;
            int result = TenantPersistenceService.CurrentWorkspace.Context.Database.ExecuteSqlCommand(insertSql, parameters);
            return result;
        }
    
    }
}