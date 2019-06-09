using System.Collections.Generic;

namespace Framework.Paging
{
    /// <summary>
    ///     Represents a subset of a collection of objects that can be individually accessed by
    ///     index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of object the collection should contain.
    /// </typeparam>
    /// <seealso cref="IEnumerable{T}" />
    public interface IPagedList<out T> : IPagedList, IEnumerable<T>
    {
        #region Public Properties

        /// <summary>
        ///     Gets the number of elements contained on this page.
        /// </summary>
        int Count { get; }

        #endregion

        #region Public Indexers

        /// <summary>
        ///     Gets the element at the specified index.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the element to get.
        /// </param>
        /// <returns>
        ///     The selected item
        /// </returns>
        T this[int index] { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a non-enumerable copy of this paged list.
        /// </summary>
        /// <returns>
        ///     A non-enumerable copy of this paged list.
        /// </returns>
        IPagedList GetMetaData();

        #endregion
    }

    /// <summary>
    ///     Represents a subset of a collection of objects that can be individually
    ///     accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    public interface IPagedList
    {
        #region Public Properties

        /// <summary>
        ///     Gets one-based index of the first item in the paged subset.
        /// </summary>
        /// <value>
        ///     The index of the first item.
        /// </value>
        int FirstItemOnPage { get; }

        /// <summary>
        ///     Gets a value indicating whether this is NOT the last subset within the superset.
        /// </summary>
        /// <value>
        ///     <value>true</value>
        ///     if this is NOT the last subset within the superset.
        /// </value>
        bool HasNextPage { get; }

        /// <summary>
        ///     Gets a value indicating whether this is NOT the first subset within the superset.
        /// </summary>
        /// <value>
        ///     <value>true</value>
        ///     if this is NOT the first subset within the superset.
        /// </value>
        bool HasPreviousPage { get; }

        /// <summary>
        ///     Gets a value indicating whether this is the first subset within the superset.
        /// </summary>
        /// <value>
        ///     <value>true</value>
        ///     if this is the first subset within the superset.
        /// </value>
        bool IsFirstPage { get; }

        /// <summary>
        ///     Gets a value indicating whether this is the last subset within the superset.
        /// </summary>
        /// <value>
        ///     <value>true</value>
        ///     if this is the last subset within the superset.
        /// </value>
        bool IsLastPage { get; }

        /// <summary>
        ///     Gets one-based index of the last item in the paged subset.
        /// </summary>
        /// <value>
        ///     The index of last item.
        /// </value>
        int LastItemOnPage { get; }

        /// <summary>
        ///     Gets total number of subsets within the superset.
        /// </summary>
        /// <value>
        ///     The total number of subsets within the superset.
        /// </value>
        int PageCount { get; }

        /// <summary>
        ///     Gets one-based index of this subset within the superset.
        /// </summary>
        /// <value>
        ///     The one-based index of this subset within the superset.
        /// </value>
        int PageNumber { get; }

        /// <summary>
        ///     Gets maximum size any individual subset.
        /// </summary>
        /// <value>
        ///     The maximum size any individual subset.
        /// </value>
        int PageSize { get; }

        /// <summary>
        ///     Gets total number of objects contained within the superset.
        /// </summary>
        /// <value>
        ///     The total number of objects contained within the superset.
        /// </value>
        int TotalItemCount { get; }

        #endregion
    }
}