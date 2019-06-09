namespace Framework.Paging
{
    /// <summary>
    /// Data structure paging information for paged query.
    /// </summary>
    public class PageInfo
    {
        #region Fields

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public int PageSize { get; set; }

        #endregion
    }
}