using System;
using System.Collections.Generic;
using System.Web;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Interfaces;

namespace QuickspatchApi.Models
{
    public abstract partial class ViewModelBase
    {
        public ViewModelBase()
        {
            if (HttpContext.Current != null && HttpContext.Current.Items.Contains("UserRoleFunctions"))
            {
                SecurityActionPermissions = HttpContext.Current.Items["UserRoleFunctions"] as List<UserRoleFunction>;
            }
        }
        public virtual string PageTitle
        {
            get;
            set;
        }
        public long Id { get; set; }
        public Guid RowGuid { get; set; }
        public bool IsDeleted { get; set; }
        private IQuickspatchPrincipal _currentUser;

        public IQuickspatchPrincipal CurrentUser
        {
            get
            {
                //if (_currentUser == null)
                //{
                //    _currentUser = DependencyResolver.Current.GetService<IAuthenticationService>().GetCurrentUser();
                //}
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }

        public long DocumentTypeId { get; set; }
        public List<UserRoleFunction> SecurityActionPermissions { get; private set; }
    }
}