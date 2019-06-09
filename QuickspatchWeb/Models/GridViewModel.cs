using System.Collections.Generic;

namespace QuickspatchWeb.Models
{
    /// <summary>
    /// Grid view model
    /// </summary>
    public class GridViewModel : ViewModelBase
    {
        public GridViewModel()
        {
            CanAddNewRecord = true;
            CanDeleteRecord = true;
            CanUpdateRecord = true;
            CanExportGrid = true;
            CanSearchGrid = true;
        }
        /// <summary>
        /// Grid id to distinguish grid in page
        /// </summary>
        public string GridId { get; set; }
        /// <summary>
        /// Model name to get data
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// Grid internal name( use in grid in grid)
        /// </summary>
        public string GridInternalName { get; set; }
        /// <summary>
        /// Advance search url
        /// </summary>
        public string AdvancedSearchUrl { get; set; }
        /// <summary>
        /// Include/Exclude active item in grid
        /// </summary>
        public bool ExcludeFilterActiveRecords { get; set; }
        /// <summary>
        /// List column in grid
        /// </summary>
        public IList<ViewColumnViewModel> ViewColumns { get; set; }

        public string UserCustom { get; set; }

        public bool UseDeleteColumn { get; set; }

        public bool CanAddNewRecord { get; set; }

        public bool CanUpdateRecord { get; set; }

        public bool CanDeleteRecord { get; set; }

        public bool CanResetPasswordRecord { get; set; }

        public int PopupWidth { get; set; }

        public int PopupHeight { get; set; }

        public string CustomHeaderTemplate { get; set; }

        public bool CanExportGrid { get; set; }
        public bool CanSearchGrid { get; set; }

       
    }
    
}