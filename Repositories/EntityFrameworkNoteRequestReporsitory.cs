using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Database.Persistance.Tenants;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;


namespace Repositories
{
    public class EntityFrameworkNoteRequestReporsitory : EntityFrameworkTenantRepositoryBase<NoteRequest>, INoteRequestRepository
    {
        public EntityFrameworkNoteRequestReporsitory(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            
        }
        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var noteRequestQueryInfo = (NoteRequestQueryInfo) queryInfo;
            var queryResult = GetAll().Where(p => p.RequestId == noteRequestQueryInfo.RequestId).Select(entity => new NoteRequestGridVo()
                               {
                                   Id = entity.Id,
                                   Description = entity.Description,
                                   CreatedDateNoFormat = entity.CreatedOn,
                                   FirstNameCreatedBy = entity.CreatedBy != null ? entity.CreatedBy.FirstName : "",
                                   MiddleNameCreatedBy = entity.CreatedBy != null ? entity.CreatedBy.MiddleName : "",
                                   LastNameCreatedBy = entity.CreatedBy != null ? entity.CreatedBy.LastName : "",
                               }).OrderBy(noteRequestQueryInfo.SortString);

            return queryResult;     
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "Id", Dir = "desc" } };
            }
            queryInfo.Sort.ForEach(x =>
            {
                if (x.Field == "CreatedDate")
                {
                    x.Field = "CreatedDateNoFormat";
                }
                else if (x.Field == "CreatedBy")
                {
                    x.Field = "LastNameCreatedBy";
                }
                else
                {
                    x.Field = String.Format("{0}", x.Field);
                }
            });
        }



        public List<NoteRequestDetail> GetNotesDetail(int requestId)
        {
            var query = from entity in GetAll().Where(o => o.RequestId == requestId)
                join courier in PersistenceService.CurrentWorkspace.Context.Users on entity.CourierId equals courier.Id
                select new NoteRequestDetail
                {
                    CreateTime = entity.CreateTime,
                    Content = entity.Description,
                    Title = "Note",
                    CourierName = courier.LastName + " " + courier.FirstName + (string.IsNullOrEmpty(courier.MiddleName) ? "" : " " + courier.MiddleName)
                };
            return query.ToList();
        }

        
    }
}
