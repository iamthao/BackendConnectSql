using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Paging
{
    /// <summary>
    ///     Represents a subset of a collection of objects that can be individually accessed by index
    ///     and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of object the collection should contain.
    /// </typeparam>
    /// <seealso cref="IPagedList{T}" />
    /// <seealso cref="List{T}" />
    public abstract class PagedListBase<T> : PagedListMetaData, IPagedList<T>
    {
        #region Fields

        /// <summary>
        ///     The subset of items contained only within this one page of the superset.
        /// </summary>
        protected readonly List<T> Subset = new List<T>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedListBase{T}" /> class.
        ///     Sets properties needed to calculate position and size data on the subset and superset.
        /// </summary>
        /// <param name="pageInfo">
        ///     Information of the desired page
        /// </param>
        /// <param name="totalItemCount">
        ///     The size of the superset.
        /// </param>
        protected internal PagedListBase(PageInfo pageInfo, int totalItemCount)
            : this(pageInfo.PageNumber, pageInfo.PageSize, totalItemCount)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedListBase{T}" /> class.
        ///     Sets properties needed to calculate position and size data on the subset and superset.
        /// </summary>
        /// <param name="pageNumber">
        ///     The one-based index of the subset of objects contained by this instance.
        /// </param>
        /// <param name="pageSize">
        ///     The maximum size of any individual subset.
        /// </param>
        /// <param name="totalItemCount">
        ///     The size of the superset.
        /// </param>
        protected internal PagedListBase(int pageNumber, int pageSize, int totalItemCount)
        {
            Initialize(pageNumber, pageSize, totalItemCount);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedListBase{T}" /> class.
        /// </summary>
        protected internal PagedListBase()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the number of elements contained on this page.
        /// </summary>
        public int Count
        {
            get { return Subset.Count; }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        ///     Gets the element at the specified index.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the element to get.
        /// </param>
        /// <returns>
        ///     The return entity.
        /// </returns>
        public T this[int index]
        {
            get { return Subset[index]; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
        /// </summary>
        /// <returns> A BasePagedList &lt; T &gt; .Enumerator for the BasePagedList &lt; T &gt; . </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Subset.GetEnumerator();
        }

        /// <summary>
        ///     Gets a non-enumerable copy of this paged list.
        /// </summary>
        /// <returns> A non-enumerable copy of this paged list. </returns>
        public IPagedList GetMetaData()
        {
            return new PagedListMetaData(this);
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
        /// </summary>
        /// <returns> A BasePagedList &lt; T &gt; .Enumerator for the BasePagedList &lt; T &gt; . </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets properties needed to calculate position and size data on the subset and superset.
        /// </summary>
        /// <param name="pageNumber">
        ///     The one-based index of the subset of objects contained by this instance.
        /// </param>
        /// <param name="pageSize">
        ///     The maximum size of any individual subset.
        /// </param>
        /// <param name="totalItemCount">
        ///     The size of the superset.
        /// </param>
        protected internal void Initialize(int pageNumber, int pageSize, int totalItemCount)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
            }

            if ((totalItemCount > 0) && (pageSize < 1))
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");
            }

            // set source to blank list if superset is null to prevent exceptions
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = TotalItemCount > 0
                ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
                : 0;
            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < PageCount;
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber >= PageCount;
            FirstItemOnPage = ((PageNumber - 1) * PageSize) + 1;
            var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
            LastItemOnPage = numberOfLastItemOnPage > TotalItemCount
                ? TotalItemCount
                : numberOfLastItemOnPage;
        }

        #endregion
    }
}