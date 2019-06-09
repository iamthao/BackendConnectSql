using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Models
{
    public class DashboardGridViewModelBase<TEntity> : ViewModelBase
                where TEntity : Entity
    {
        public override string PageTitle
        {
            get
            {
                return typeof(TEntity).Name;
            }
        }

        private GridViewModel _gridViewModel;
        /// <summary>
        /// Gets or sets the grid view model.
        /// </summary>
        [JsonIgnore]
        public virtual GridViewModel GridViewModel
        {
            get { return _gridViewModel; }
            set
            {
                _gridViewModel = value;
                var entityName = typeof(TEntity).Name;
                if (string.IsNullOrEmpty(_gridViewModel.ModelName))
                {
                    
                    _gridViewModel.ModelName = entityName;
                    _gridViewModel.GridInternalName = entityName;
                }
                if (_gridViewModel.DocumentTypeId == 0)
                {
                    DocumentTypeKey documentType;
                    Enum.TryParse(entityName, out documentType);
                    _gridViewModel.DocumentTypeId = (int)documentType;
                }
                
                _gridViewModel.PageTitle = PageTitle;
                _gridViewModel.CurrentUser = DependencyResolver.Current.GetService<IAuthenticationService>().GetCurrentUser();
            }
        }
    }    
}