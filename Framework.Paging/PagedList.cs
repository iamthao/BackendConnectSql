using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Paging
{
    /// <summary>
    ///     Represents a subset of a collection of objects that can be individually accessed by
    ///     index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of object the collection should contain.
    /// </typeparam>
    /// <seealso cref="IPagedList{T}" />
    /// <seealso cref="PagedListBase{T}" />
    /// <seealso cref="List{T}" />
    public class PagedList<T> : PagedListBase<T>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedList{T}" /> class that divides
        ///     the supplied superset into subsets the size of the supplied pageSize. The instance
        ///     then only contains the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">
        ///     The collection of objects to be divided into subsets. If the
        ///     collection implements <see cref="IQueryable{T}" />, it will be treated as such.
        /// </param>
        /// <param name="pageInfo">
        ///     Paging information
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The specified index cannot be less than zero.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The specified page size cannot be less than one.
        /// </exception>
        public PagedList(IQueryable<T> superset, PageInfo pageInfo)
        {
            var pageNumber = 1;
            var pageSize = int.MaxValue;
            if (pageInfo != null)
            {
                pageNumber = pageInfo.PageNumber;
                pageSize = pageInfo.PageSize;
                // totalItemCount = superset == null ? 0 : superset.Count();
            }

            var totalItemCount = superset == null ? 0 : superset.Count();
            Initialize(pageNumber, pageSize, totalItemCount);

            // add items to internal list
            Paging(superset, pageNumber, pageSize);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedList{T}" /> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only contains the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">
        ///     The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}" />, it will be treated as such.
        /// </param>
        /// <param name="pageNumber">
        ///     The one-based index of the subset of objects to be contained by this instance.
        /// </param>
        /// <param name="pageSize">
        ///     The maximum size of any individual subset.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The specified index cannot be less than zero.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The specified page size cannot be less than one.
        /// </exception>
        public PagedList(IQueryable<T> superset, int pageNumber, int pageSize)
            : base(pageNumber, pageSize, superset == null ? 0 : superset.Count())
        {
            // add items to internal list
            Paging(superset, pageNumber, pageSize);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedList{T}" /> class that divides
        ///     the supplied superset into subsets the size of the supplied pageSize. The instance
        ///     then only contains the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">
        ///     The collection of objects to be divided into subsets. If the collection
        ///     implements <see cref="IQueryable{T}" />, it will be treated as such.
        /// </param>
        /// <param name="pageNumber">
        ///     The one-based index of the subset of objects to be contained by this instance.
        /// </param>
        /// <param name="pageSize">
        ///     The maximum size of any individual subset.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The specified index cannot be less than zero.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The specified page size cannot be less than one.
        /// </exception>
        public PagedList(IEnumerable<T> superset, int pageNumber, int pageSize)
            : base(pageNumber, pageSize, superset == null ? 0 : superset.Count())
        {
            // add items to internal list
            Paging(superset, pageNumber, pageSize);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedList{T}" /> class that
        ///     divides the supplied superset into subsets the size of the supplied pageSize.
        ///     The instance then only contains the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">
        ///     The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}" />, it will be treated as such.
        /// </param>
        /// <param name="pageInfo">
        ///     Paging information
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The specified index cannot be less than zero.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The specified page size cannot be less than one.
        /// </exception>
        public PagedList(IEnumerable<T> superset, PageInfo pageInfo)
            : this(superset, pageInfo.PageNumber, pageInfo.PageSize)
        {
        }

        /// <summary>
        ///     Sets properties needed to calculate position and size data on the subset and superset.
        /// </summary>
        /// <param name="superset">
        ///     The collection of objects to be divided into subsets. If the collection
        ///     implements <see cref="IEnumerable{T}" />, it will be treated as such.
        /// </param>
        /// <param name="pageNumber">
        ///     The one-based index of the subset of objects contained by this instance.
        /// </param>
        /// <param name="pageSize">
        ///     The maximum size of any individual subset.
        /// </param>
        private void Paging(IEnumerable<T> superset, int pageNumber, int pageSize)
        {
            // add items to internal list
            if (superset != null && TotalItemCount > 0)
            {
                Subset.AddRange(
                    pageNumber == 1
                        ? superset.Skip(0).Take(pageSize).ToList()
                        : superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
            }
        }

        #endregion
    }
}