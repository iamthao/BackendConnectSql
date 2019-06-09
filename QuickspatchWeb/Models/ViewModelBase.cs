using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using Framework.DomainModel.Entities.Security;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using ServiceLayer.Common;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Authentication;

namespace QuickspatchWeb.Models
{
    public partial class ViewModelBase
    {
        public ViewModelBase()
        {
            //if (HttpContext.Current != null && HttpContext.Current.Items.Contains("UserRoleFunctions"))
            //{
            //    SecurityActionPermissions = HttpContext.Current.Items["UserRoleFunctions"] as List<UserRoleFunction>;
            //}
        }
        public virtual string PageTitle
        {
            get;
            set;
        }
        public int Id { get; set; }
        public Guid RowGuid { get; set; }
        public bool IsDeleted { get; set; }
        private IQuickspatchPrincipal _currentUser;
        public IQuickspatchPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = DependencyResolver.Current.GetService<IAuthenticationService>().GetCurrentUser();
                }
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }

        private string _displayNameForCourier;
        public string DisplayNameForCourier
        {
            get
            {
                //if (string.IsNullOrEmpty(_displayNameForCourier))
                //{
                //    _displayNameForCourier = DependencyResolver.Current.GetService<IFranchiseeTenantService>().GetDisplayNameForCourier();
                //}
                return "Mobile User";
            }
            //set
            //{
            //    _displayNameForCourier = value;
            //}
        }


        public MenuViewModel MenuViewModel
        {
            get
            {
                var menuViewModel = MenuExtractData.Instance.GetMenuViewModel(CurrentUser.User.UserRoleId.GetValueOrDefault());
                return menuViewModel;
            }
        }

        public int DocumentTypeId { get; set; }
        public Collection<FooterAction> FooterActions { get; set; }
        //public List<UserRoleFunction> SecurityActionPermissions { get; private set; }
        public object AddFooterAction(string text, string icon, FooterActionEnum function, string controllerScriptId, bool ignoreDirty = true, OperationAction securityMapAction = OperationAction.None)
        {
            var permission = true;
            if (FooterActions == null)
            {
                FooterActions = new Collection<FooterAction>();
            }
            //if (securityMapAction != OperationAction.None && SecurityActionPermissions != null)
            //{
            //    permission = SecurityActionPermissions.Any(s => s.SecurityOperationId == (int)securityMapAction);
            //}
            FooterActions.Add(new FooterAction
            {
                Action = function,
                Icon = icon,
                Text = text,
                //Text = text.ToUpperInvariant(),
                IgnoreDirty = ignoreDirty,
                Permission = permission,
                ControllerScripId = controllerScriptId
            });
            return null;
        }
    }

    public class FooterAction
    {
        public string Icon { get; set; }
        public string Text { get; set; }
        public FooterActionEnum Action { get; set; }
        public bool IgnoreDirty { get; set; }
        public bool Permission { get; set; }
        public string ActionMethod
        {
            get
            {
                switch (Action)
                {
                    case FooterActionEnum.Cancel:
                        return "Cancel('" + ControllerScripId + "');";
                    case FooterActionEnum.Save:
                        return "Save('" + ControllerScripId + "');";
                }
                return "";
            }
        }
        public string ActionMethodKey
        {
            get
            {
                switch (Action)
                {
                    case FooterActionEnum.Cancel:
                        return "FOOTER_ACTION_CANCEl";
                    case FooterActionEnum.Save:
                        return "FOOTER_ACTION_SAVE";
                }
                return "";
            }
        }
        public string ClassType
        {
            get
            {
                switch (Action)
                {
                    case FooterActionEnum.Cancel:
                        return "btn-default";
                    case FooterActionEnum.Save:
                        return "btn-primary";
                }
                return "";
            }
        }
        public string ControllerScripId { get; set; }
    }

    public enum FooterActionEnum
    {
        Save,
        Cancel
    }
}