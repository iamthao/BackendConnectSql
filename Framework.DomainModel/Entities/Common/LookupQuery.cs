﻿using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Framework.DomainModel.Entities.Common
{
    public class LookupQuery
    {
        public LookupQuery()
        {
            Take = 50;
        }
        private Collection<LookupHierarchyItem> _hierachyItems;
        public string Query { get; set; }
        public string ParentItems { get; set; }

        /// <summary>
        /// List of parent fields, ids that wants to filter the lookup for.
        /// </summary>
        public Collection<LookupHierarchyItem> HierachyItems
        {
            get
            {
                if (_hierachyItems == null)
                {
                    if (ParentItems != null)
                        _hierachyItems = JsonConvert.DeserializeObject<Collection<LookupHierarchyItem>>(ParentItems);
                    else
                        _hierachyItems = new Collection<LookupHierarchyItem>();
                }

                return _hierachyItems;
            }
            set
            {
                _hierachyItems = value;
            }

        }

        public int Take { get; set; }

        public string MasterFilterFieldName { get; set; }

        public bool IncludeInactiveRecords { get; set; }
        public int Id { get; set; } // current id of lookup
        public bool IncludeCurrentRecord { get; set; }
        public string CustomeFilterKey { get; set; }
        public string CustomeFilterValue { get; set; }
    }

    public class LookupHierarchyItem
    {
        public string Name;
        public string Value;
        public bool IgnoredFilter { get; set; }
    }

    public class LookupItem
    {
        public int Id { get; set; }
        public LookupHierarchyItem HierachyItem
        {
            get
            {
                try
                {
                    return FilterItem == null ? null : JsonConvert.DeserializeObject<LookupHierarchyItem>(FilterItem);
                }
                catch
                {
                    return null;
                }
            }
        }
        public string FilterItem
        {
            get;
            set;
        }
    }
}