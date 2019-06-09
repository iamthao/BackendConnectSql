using Database.Persistance.Tenants;
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
    public class EntityFrameworkLocationRepository : EntityFrameworkTenantRepositoryBase<Location>, ILocationRepository
    {
        public EntityFrameworkLocationRepository(ITenantPersistenceService persistenceService)
            :base(persistenceService)
        {
            SearchColumns.Add("Name");
            SearchColumns.Add("FullAddressSearch");
            
            DisplayColumnForCombobox = "Name";
        }
        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = (from entity in GetAll()
                               select new { entity }).Select(s => new LocationGridVo
                               {
                                   Id = s.entity.Id,
                                   Name = s.entity.Name,
                                   Address1 =s.entity.Address1,
                                   Address2= s.entity.Address2,
                                   City = s.entity.City,
                                   Zip = s.entity.Zip,
                                   FullAddressSearch = s.entity.Address1
                                   + (!string.IsNullOrEmpty(s.entity.Address2)?", "+s.entity.Address2:"")
                                   + (!string.IsNullOrEmpty(s.entity.City) ? ", " + s.entity.City : "")
                                   + (!string.IsNullOrEmpty(s.entity.StateOrProvinceOrRegion) ? ", " + s.entity.StateOrProvinceOrRegion : "")
                                   + (!string.IsNullOrEmpty(s.entity.Zip) ? " " + s.entity.Zip : "")
                                   + (s.entity.CountryOrRegion != null && !string.IsNullOrEmpty(s.entity.CountryOrRegion.Name) ? ", " + s.entity.CountryOrRegion.Name : ""),
                                   
                                   AvailableTimeNoFormat = s.entity.AvailableTime,

                                   OpenHourNoFormat = s.entity.OpenHour,

                                   CloseHourNoFormat = s.entity.CloseHour,
                                   //
                                   StateOrProvinceOrRegion =  s.entity.StateOrProvinceOrRegion,
                                   CountryOrRegion = s.entity.CountryOrRegion != null ? s.entity.CountryOrRegion.Name : "",

                               }).OrderBy(queryInfo.SortString);

            return queryResult;     
        }

        public Location GetLocation()
        {
            return GetAll().SingleOrDefault();
        }
        
        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "Name", Dir = "" } };
            }
            queryInfo.Sort.ForEach(x =>
            {
                if (x.Field == "Name")
                {
                    x.Field = "Name";
                }

                if (x.Field == "FullAddress")
                {
                    x.Field = "Address1";
                }

                if (x.Field == "AvailableTime")
                {
                    x.Field = "AvailableTimeNoFormat";
                }

                if (x.Field == "OpenHour")
                {
                    x.Field = "OpenHourNoFormat";
                }
                if (x.Field == "CloseHour")
                {
                    x.Field = "CloseHourNoFormat";
                }
                else
                {
                    x.Field = String.Format("{0}", x.Field);
                }
            });
        }
        protected override string BuildLookupCondition(LookupQuery query)
        {
            var where = new StringBuilder();
            @where.Append("(");
            var innerWhere = new List<string>();
            var queryDisplayName = String.Format("Name.Contains(\"{0}\")", query.Query);
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

        public List<LocationDefaultVo> GetListLocation()
        {
            var query = GetAll().Select(s => new LocationDefaultVo
            {
                Id = s.Id,
                Name = s.Name,
                Address1 = s.Address1,
                Address2 = s.Address2,
                StateOrProvinceOrRegion = s.StateOrProvinceOrRegion,
                Zip = s.Zip,
                CountryOrRegion = s.CountryOrRegion != null ? s.CountryOrRegion.Name : "",
            }).ToList();

            query.Add(new LocationDefaultVo
            {
                Id = 0,
                Name = "Select Location"
            });
            return query;
        }

        public LatLngVo GetLatLng(int id)
        {
            return GetAll().Where(o => o.Id == id).Select(s => new LatLngVo {Lat = s.Lat, Lng = s.Lng}).FirstOrDefault();
        }
    }
}
