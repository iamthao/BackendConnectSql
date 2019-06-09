using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Paging;
using Framework.QueryEngine;

namespace Consume.ServiceLayer
{
    public abstract class WebApiConsumeMasterFileService<TEntity> : IWebApiConsumeMasterFileService<TEntity>
        where TEntity : DtoBase
    {
        public abstract string WebApiBaseUrl { get; }

        public string WebApiUrl
        {
            get { return ConfigurationManager.AppSettings["WebApiUrl"] + "api/" + WebApiBaseUrl + "/"; }
        }

        public virtual  TEntity GetById( int id)
        {
            return  SendMessageToWebApiAndReturnValue<TEntity>( "GetById", id);
        }

        public virtual  TEntity Add( TEntity entity)
        {
            return  SendMessageToWebApiAndReturnValue<TEntity>( "Add", entity);
        }

        public virtual  byte[] Update( TEntity entity)
        {
            return  SendMessageToWebApiAndReturnValue<byte[]>( "Update", entity);
        }

        public virtual  TEntity Single( Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete( TEntity model)
        {
            return SendMessageToWebApiAndReturnValue<bool>( "Delete", model);
        }

        public virtual bool DeleteById( int id)
        {
            return SendMessageToWebApiAndReturnValue<bool>( "DeleteById", id);
        }


        public virtual  IList<TEntity> ListAll()
        {
            return  SendMessageToWebApiAndReturnValue<IList<TEntity>>( "ListAll");
        }

        public virtual int Count( Expression<Func<TEntity, bool>> @where = null)
        {
            throw new NotImplementedException();
        }

        public virtual  bool CheckExist( Expression<Func<TEntity, bool>> @where = null)
        {
            throw new NotImplementedException();
        }

        public virtual  IList<TEntity> Get( Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual  IList<TEntity> Get<TOrderby>(
            Expression<Func<TEntity, bool>> filter = null, bool isDescending = false,
            Expression<Func<TEntity, TOrderby>> order = null,
            bool isNoTracking = false, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public virtual  IPagedList<TEntity> GetPaged<TOrderby>( PageInfo pageInfo,
            Expression<Func<TEntity, bool>> filter = null, bool isDescending = false,
            Expression<Func<TEntity, TOrderby>> order = null,
            bool isNoTracking = false, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public virtual  IList<TEntity> GetDescending<TOrderby>(
            Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderby>> order, bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public virtual  IList<TEntity> GetAscending<TOrderby>(
            Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderby>> order, bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public virtual  IPagedList<TEntity> GetPagedDescending<TOrderby>(
            PageInfo pageinfo, Expression<Func<TEntity, TOrderby>> order, bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public virtual  IPagedList<TEntity> GetPagedAscending<TOrderby>(
            Expression<Func<TEntity, TOrderby>> order, PageInfo pageinfo, bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            throw new NotImplementedException();
        }

        public virtual  PagedList<TEntity> Get( IQueryOption<TEntity> queryOption,
            PageInfo pageInfo)
        {
            throw new NotImplementedException();
        }

        public virtual  TResult Query<TResult>(
            Func<IQueryable<TEntity>, TResult> resultTransformer, IQueryOption<TEntity> queryOption)
        {
            throw new NotImplementedException();
        }

        public virtual  IList<TEntity> Get( IQueryOption<TEntity> queryOption)
        {
            throw new NotImplementedException();
        }

        public virtual  TEntity FirstOrDefault(
            Expression<Func<TEntity, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public virtual List<TEntity> GetByPage( QueryInfo queryInfo,
            Expression<Func<TEntity, object>> orderBy, out int totalRowCount)
        {
            throw new NotImplementedException();
        }

        public virtual bool InsertOrUpdate( TEntity entity)
        {
            return SendMessageToWebApiAndReturnValue<bool>( "InsertOrUpdate", entity);
        }

        public virtual  List<int> DeleteById( IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public virtual  bool DeleteAll( IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAll( Expression<Func<TEntity, bool>> @where = null)
        {
            throw new NotImplementedException();
        }

        public virtual  List<LookupItemVo> GetLookup( LookupQuery query,
            Func<TEntity, LookupItemVo> selector)
        {
            throw new NotImplementedException();
        }

        public virtual  LookupItemVo GetLookupItem( LookupItem lookupItem,
            Func<TEntity, LookupItemVo> selector)
        {
            throw new NotImplementedException();
        }

        public  dynamic GetDataForGrid( IQueryInfo queryInfo)
        {
            return  SendMessageToWebApiAndReturnValue<dynamic>( "GetDataForGrid", queryInfo);
        }

        protected virtual void SetClientHeader(HttpClient client,string baseAddressRewrite="")
        {
            if (!string.IsNullOrEmpty(baseAddressRewrite))
            {
                client.BaseAddress = new Uri(baseAddressRewrite);
            }
            else
            {
                client.BaseAddress = new Uri(WebApiUrl);    
            }
            HttpCookie tokenClaimCookie = GetLastCookie(ClaimsDeclaration.TokenClaim);
            if (tokenClaimCookie == null)
            {
                var claimException = new InvalidClaimsException("InvalidAccessToken")
                {
                    QuickspatchUserName = string.Empty
                };
                throw claimException;
            }
            string token = tokenClaimCookie.Value;
            client.SetBearerToken(token);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private HttpCookie GetLastCookie(string name)
        {
            HttpCookie lastCookie = null;
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                if (HttpContext.Current.Request.Cookies[i] != null && HttpContext.Current.Request.Cookies[i].Name.Equals(name))
                {
                    if (lastCookie == null || lastCookie.Expires < HttpContext.Current.Request.Cookies[i].Expires)
                    {
                        lastCookie = HttpContext.Current.Request.Cookies[i];
                    }
                }
            }
            return lastCookie;
        }

        protected virtual TResult SendMessageToWebApiAndReturnValue<TResult>(
            string method, object data = null,string baseAddressRewrite="")
        {
            using (var client = new HttpClient())
            {
                SetClientHeader(client, baseAddressRewrite);
                var task = client.PostAsJsonAsync(method, data).ContinueWith(x => x.Result);
                task.Wait();

                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    return response.Content.ReadAsAsync<TResult>().Result;
                }
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    FeedbackViewModel dataReturn = response.Content.ReadAsAsync<FeedbackViewModel>().Result;
                    throw new WebApiErrorException(dataReturn);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnAuthorizedAccessException("Unauthorized");
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new UnAuthorizedAccessException("ForbiddenAccess");
                }
                if (response.StatusCode == HttpStatusCode.ExpectationFailed)
                {
                    throw new UnAuthorizedAccessException("ExpectationFailed");
                }
            }
            return default(TResult);
        }
    }
}
