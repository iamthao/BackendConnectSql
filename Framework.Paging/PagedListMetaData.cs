namespace Framework.Paging
{
    /// <summary>
    ///     Non-enumerable version of the PagedList class.
    /// </summary>
    public class PagedListMetaData : IPagedList
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedListMetaData" /> class.
        ///     Non-enumerable version of the PagedList class.
        /// </summary>
        /// <param name="pagedList">
        ///     A PagedList (likely enumerable) to copy metadata from.
        /// </param>
        public PagedListMetaData(IPagedList pagedList)
        {
            PageCount = pagedList.PageCount;
            TotalItemCount = pagedList.TotalItemCount;
            PageNumber = pagedList.PageNumber;
            PageSize = pagedList.PageSize;
            HasPreviousPage = pagedList.HasPreviousPage;
            HasNextPage = pagedList.HasNextPage;
            IsFirstPage = pagedList.IsFirstPage;
            IsLastPage = pagedList.IsLastPage;
            FirstItemOnPage = pagedList.FirstItemOnPage;
            LastItemOnPage = pagedList.LastItemOnPage;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PagedListMetaData" /> class.
        ///     Protected constructor that allows for instantiation without passing in a separate list.
        /// </summary>
        protected PagedListMetaData()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets one-based index of the first item in the paged subset.
        /// </summary>
        /// <value>
        ///     The index of the first item.
        /// </value>
        public int FirstItemOnPage { get; protected set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this is NOT the last subset within the superset.
        /// </summary>
        /// <value>
        ///     <value>true</value>
        ///     if this is NOT the last subset within the superset.
        /// </value>
        public bool HasNextPage { get; protected set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this is NOT the first subset within the superset.
        /// </summary>
        /// <value>
        ///     <value>true</value>
        ///     if this is NOT the first subset within the superset.
        /// </value>
        public bool HasPreviousPage { get; protected set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this is the first subset within the superset.
        /// </summary>
        /// <value>
        ///     <value>true</value>
        ///     if this is the first subset within the superset.
        /// </value>
        public bool IsFirstPage { get; protected set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this is the last subset within the superset.
        /// </summary>
        /// <value>
        ///     <value>true</value>
        ///     if this is the last subset within the superset.
        /// </value>
        public bool IsLastPage { get; protected set; }

        /// <summary>
        ///     Gets or sets one-based index of the last item in the paged subset.
        /// </summary>
        /// <value>
        ///     The index of last item.
        /// </value>
        public int LastItemOnPage { get; protected set; }

        /// <summary>
        ///     Gets or sets total number of subsets within the superset.
        /// </summary>
        /// <value>
        ///     The total number of subsets within the superset.
        /// </value>
        public int PageCount { get; protected set; }

        /// <summary>
        ///     Gets or sets one-based index of this subset within the superset.
        /// </summary>
        /// <value>
        ///     The one-based index of this subset within the superset.
        /// </value>
        public int PageNumber { get; protected set; }

        /// <summary>
        ///     Gets or sets maximum size any individual subset.
        /// </summary>
        /// <value>
        ///     The maximum size any individual subset.
        /// </value>
        public int PageSize { get; protected set; }

        /// <summary>
        ///     Gets or sets total number of objects contained within the superset.
        /// </summary>
        /// <value>
        ///     The total number of objects contained within the superset.
        /// </value>
        public int TotalItemCount { get; protected set; }

        #endregion
    }
}