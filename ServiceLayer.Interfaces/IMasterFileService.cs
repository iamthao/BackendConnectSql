using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Paging;
using Framework.QueryEngine;

namespace ServiceLayer.Interfaces
{
    public interface IMasterFileService<TEntity> where TEntity : Entity
    {
        TEntity GetById(int id);
        TEntity Add(TEntity model);
        TEntity Update(TEntity model);

        /// <summary>
        /// Get entity by expression
        /// </summary>
        /// <param name="predicate">Condition expression to search</param>
        /// <returns>First entity</returns>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        void Delete(TEntity model);

        /// <summary>
        ///     Delete an entity by its id.
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns>Affected record count.</returns>
        void DeleteById(int id);


        /// <summary>
        ///     List all entities in database
        /// </summary>
        /// <returns>List of entities</returns>
        IList<TEntity> ListAll();

        /// <summary>
        ///     The count number of entity.
        /// </summary>
        /// <param name="where">searching condition</param>
        /// <returns>number of record </returns>
        int Count(Expression<Func<TEntity, bool>> @where = null);

        /// <summary>
        ///     The check entity exist.
        /// </summary>
        /// <param name="where">Condition to check</param>
        /// <returns>true if exist, false if not</returns>
        bool CheckExist(Expression<Func<TEntity, bool>> @where = null);


        /// <summary>
        ///     Get entity by expression
        /// </summary>
        /// <param name="predicate">condition to get</param>
        /// <returns>List of  entities that qualify condition</returns>
        IList<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Get entity by expression
        /// </summary>
        /// <typeparam name="TOrderby">order field data type</typeparam>
        /// <param name="filter">filter condition</param>
        /// <param name="isDescending">Is oder by Descending</param>
        /// <param name="order">field that application uses to order</param>
        /// <param name="isNoTracking">Load data without tracking</param>
        /// <param name="includeExpressions">List of field that eager load</param>
        /// <returns>List of entities</returns>
        IList<TEntity> Get<TOrderby>(Expression<Func<TEntity, bool>> filter = null, bool isDescending = false,
            Expression<Func<TEntity, TOrderby>> order = null,
            bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions);

        /// <summary>
        ///     Load paged data from database
        /// </summary>
        /// <typeparam name="TOrderby">order by field data type</typeparam>
        /// <param name="pageInfo">Paging infomation</param>
        /// <param name="filter">Loading condition</param>
        /// <param name="isDescending">is order by descending</param>
        /// <param name="order">The field that used to order</param>
        /// <param name="isNoTracking">Load data without tracking</param>
        /// <param name="includeExpressions">List of eager load relation object</param>
        /// <returns>List of entities</returns>
        IPagedList<TEntity> GetPaged<TOrderby>(PageInfo pageInfo,
            Expression<Func<TEntity, bool>> filter = null,
            bool isDescending = false,
            Expression<Func<TEntity, TOrderby>> order = null,
            bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions);

        /// <summary>
        ///     Get paged Descending entities
        /// </summary>
        /// <typeparam name="TOrderby"></typeparam>
        /// <param name="filter">Filtering conditions</param>
        /// <param name="order">>Ordering expression</param>
        /// <param name="isNoTracking">Should track returns enities in dbContext</param>
        /// <param name="includeExpressions">Eager load options</param>
        /// <returns>List of entities</returns>
        IList<TEntity> GetDescending<TOrderby>(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderby>> order,
            bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions);


        /// <summary>
        ///     Get paged Ascending entities
        /// </summary>
        /// <typeparam name="TOrderby">Order field of entity</typeparam>
        /// <param name="filter">Filtering condition</param>
        /// <param name="order">Order expression</param>
        /// <param name="isNoTracking">Should track returns enities in dbContext</param>
        /// <param name="includeExpressions">Eager load options</param>
        /// <returns>List of entities</returns>
        IList<TEntity> GetAscending<TOrderby>(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderby>> order,
            bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions);

        /// <summary>
        ///     Get paged Descending entities
        /// </summary>
        /// <typeparam name="TOrderby">Order field of entity</typeparam>
        /// <param name="pageinfo">Order expression</param>
        /// <param name="order">Order expression</param>
        /// <param name="isNoTracking">Should track returns enities in dbContext</param>
        /// <param name="includeExpressions">Eager load options</param>
        /// <returns>List of entities</returns>
        IPagedList<TEntity> GetPagedDescending<TOrderby>(PageInfo pageinfo,
            Expression<Func<TEntity, TOrderby>> order,
            bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[]
                includeExpressions);


        /// <summary>
        ///     Get paged Ascending entities
        /// </summary>
        /// <typeparam name="TOrderby">Order field of entity</typeparam>
        /// <param name="order">Order expression</param>
        /// <param name="pageinfo">Paged infomation</param>
        /// <param name="isNoTracking">Should track returns enities in dbContext</param>
        /// <param name="includeExpressions">Eager load options</param>
        /// <returns>List of entities</returns>
        IPagedList<TEntity> GetPagedAscending<TOrderby>(
            Expression<Func<TEntity, TOrderby>> order,
            PageInfo pageinfo,
            bool isNoTracking = false,
            params Expression<Func<TEntity, object>>[] includeExpressions);

        /// <summary>
        ///     Get entity by Query option
        /// </summary>
        /// <param name="queryOption">
        ///     QueryOption <see cref="IQueryOption{TEntity}" />
        /// </param>
        /// <param name="pageInfo">
        ///     Page infomation<see cref="PageInfo" />
        /// </param>
        /// <returns>List of entities</returns>
        PagedList<TEntity> Get(IQueryOption<TEntity> queryOption, PageInfo pageInfo);


        /// <summary>
        ///     Load data and transform data to expected result
        /// </summary>
        /// <typeparam name="TResult">Expected result</typeparam>
        /// <param name="resultTransformer">Funtion that transform data from source to destination</param>
        /// <param name="queryOption"></param>
        /// <returns></returns>
        TResult Query<TResult>(Func<IQueryable<TEntity>, TResult> resultTransformer,
            IQueryOption<TEntity> queryOption);


        /// <summary>
        ///     Load data from database by IQueryOption
        /// </summary>
        /// <param name="queryOption">
        ///     Wraper of query infomation <see cref="IQueryOption{TEntity}" />
        ///     ,this could be seaching condition, order by.... This is a decoration pattern
        /// </param>
        /// <returns>List of entities</returns>
        IList<TEntity> Get(IQueryOption<TEntity> queryOption);


        /// <summary>
        ///     Get entity by expression
        /// </summary>
        /// <param name="predicate">Condition expression to search</param>
        /// <returns>First entity or default</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        ///     Specific support for get Dynamic View
        /// </summary>
        /// <param name="queryInfo">
        ///     QueryInfo <see cref="QueryInfo" />
        /// </param>
        /// <param name="totalRowCount">Number of rows</param>
        /// <param name="filter"></param>
        /// <param name="orderBy">Order by expression</param>

        /// <returns>List of entities</returns>
        List<TEntity> GetByPage(QueryInfo queryInfo,
            out int totalRowCount,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null);


        /// <summary>
        ///     Insert or Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void InsertOrUpdate(TEntity entity);

        /// <summary>
        ///     Delete a list of entities
        /// </summary>
        /// <param name="ids">List of entities</param>
        /// <returns>Affected record count.</returns>
        List<int> DeleteById(IEnumerable<int> ids);

        /// <summary>
        ///     Delete a list of entity
        /// </summary>
        /// <param name="entities">List of entities to delete</param>
        void DeleteAll(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Delete entity base on condition
        /// </summary>
        /// <param name="where">Condition to delete</param>
        void DeleteAll(Expression<Func<TEntity, bool>> @where = null);

        /// <summary>
        ///     Get item for lookup
        /// </summary>
        /// <returns></returns>
        List<LookupItemVo> GetLookup(LookupQuery query, Func<TEntity, LookupItemVo> selector);

        /// <summary>
        /// Get parent lookup item from child item
        /// </summary>
        /// <param name="lookupItem"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        LookupItemVo GetLookupItem(LookupItem lookupItem, Func<TEntity, LookupItemVo> selector);

        /// <summary>
        /// Get data for grid
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        dynamic GetDataForGridMasterfile(IQueryInfo queryInfo);

        void ThrowCustomValidation(string mess);
    }
}