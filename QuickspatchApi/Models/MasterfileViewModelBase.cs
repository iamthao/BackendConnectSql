using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Framework.DomainModel;
using Framework.Utility;
using Newtonsoft.Json;

namespace QuickspatchApi.Models
{
    public class MasterfileViewModelBase<TEntity> : ViewModelBase
        where TEntity : Entity
    {
        [Timestamp]
        public byte[] LastModified { get; set; }
        public DashboardSharedViewModel SharedViewModel { get; set; }


        #region Map Parameters
        //TODO: temporary this is virtual to prevent exception in build, wating for all childs refactoring, 
        //this method should be abstract to force implementation in child
        public virtual void MapFromClientParameters(MasterfileParameter parameters)
        {
            throw new NotImplementedException();
        }


        public virtual void ProcessFromClientParameters(MasterfileParameter parameters)
        {
            MapFromClientParameters(parameters);
        }

        public virtual TViewModel MapFromClientParameters<TViewModel>(MasterfileParameter parameters) where TViewModel : DashboardSharedViewModel
        {
            if (string.IsNullOrEmpty(parameters.SharedParameter))
                return null;


            var jSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            jSettings.Converters.Add(new DefaultWrongFormatDeserialize());
            var sharedModel = JsonConvert.DeserializeObject<TViewModel>(parameters.SharedParameter, jSettings
                );
            if (sharedModel != null)
            {
                sharedModel.CustomMappingRule();
            }
            return sharedModel;
        }

        #endregion

    }


    public class MasterfileParameter
    {
        public string SharedParameter { get; set; }
    }

    public class DashboardSharedViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether create mode.
        /// </summary>
        public bool CreateMode { get; set; }


        public ShowPopupType ShowPopupType { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        [Timestamp]
        public byte[] LastModified { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual void CustomMappingRule()
        {

        }
    }

    public enum ShowPopupType
    {
        ComboBox = 1,
        ComboBoxForTreating = 2,
        Grid = 3,
    }
}