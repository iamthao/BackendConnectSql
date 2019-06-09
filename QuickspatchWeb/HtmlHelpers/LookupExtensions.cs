using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using QuickspatchWeb.Models;

namespace QuickspatchWeb.HtmlHelpers
{
    public static class LookupExtensions
    {
        public static MvcHtmlString Lookup(this HtmlHelper htmlHelper, string lookupId,
           string label,
           string modelName,
           string dataBindingValue,
           int col = 6,
           string url = "",
           bool populatedByChildren = false,
           string hierarchyGroupName = "",
           string urlToReadData = "", string urlToGetLookupItem = "", bool isRequired = false, object htmlAttribute = null, int currentId = 0, object dataSource = null, int heightLookup = 250,
           string customParams = "",
           string angularLookupController = "",
           string isShow = "true",
          string addLookupPopupFunction = "",
           string editLookupPopupFunction = "",
           string moreClass = "",
           bool showAddEdit = true,
           string enable = "true")
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", lookupId);

            if (attribute.ContainsKey("style"))
            {
                if (!attribute["style"].ToString().Contains("width"))
                {
                    attribute["style"] = attribute["style"] + "width: 270px;";
                }
            }
            else
            {
                attribute.Add("style", "");
            }

            if (isRequired)
            {
                attribute["required"] = "required";
            }

            attribute["style"] = attribute["style"] + "display: none;";

            var model = new LookupViewModel
            {
                ID = lookupId,
                Label = label,
                ModelName = modelName,
                UrlToReadData = urlToReadData,
                UrlToGetLookupItem = urlToGetLookupItem,
                CurrentId = currentId,
                HtmlAttributes = attribute,
                HierarchyGroupName = hierarchyGroupName,
                PopulatedByChildren = populatedByChildren,
                DataSource = new List<object>(),
                HeightLookup = heightLookup,
                DataBindingValue = dataBindingValue,
                Required = isRequired,
                Col = col,
                CustomParams = customParams,
                AngularLookupController = angularLookupController,
                IsShow = isShow,
                AddLookupPopupFunction = addLookupPopupFunction,
                EditLookupPopupFunction = editLookupPopupFunction,
                MoreClass = moreClass,
                ShowAddEdit = showAddEdit,
                Enable = enable
            };
            if (dataSource != null) model.DataSource.Add(dataSource);
            if (string.IsNullOrEmpty(url))
            {
                url = "~/Views/Shared/Lookup/_Lookup.cshtml";
            }
            return htmlHelper.Partial(url, model);
        }

    }
}