using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using QuickspatchWeb.Models.Date;
using QuickspatchWeb.Models.Input;

namespace QuickspatchWeb.HtmlHelpers
{
    public static class InputExtensions
    {
        public static MvcHtmlString GenericDualListBox(this HtmlHelper htmlHelper, string modelName, int entityId, string avaliableAction, string selectedAction,
                                                                                                           string queryEntityName, string headerText = "")
        {
            var model = new DualListBoxViewModel
            {
                ControlId = modelName,
                ModelName = modelName,
                GetAllUrl = string.Format("/{0}/{1}", modelName, avaliableAction),
                GetSelectedUrl = string.Format("/{0}/{1}?{2}= {3}", modelName, selectedAction, queryEntityName, entityId),
                HeaderText = headerText

            };
            var mvcHtmlString = htmlHelper.Partial("~/Views/Shared/DualListBox/_DualListBoxViewModel.cshtml", model);
            return mvcHtmlString;
        }

        public static MvcHtmlString CustomTextBoxWithButton(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6, bool required = false, int length = Int32.MaxValue,
            bool isReadonly = false, string cssClass = "k-input", object htmlAttribute = null, string placeHolderText = "", string buttonFunctionName = "", string buttonFunctionText = "")
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);

            var viewModel = new InputTextViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                Length = length,
                ReadOnly = isReadonly,
                PlaceHolderText = placeHolderText,
                TextboxType = "type=text",
                Col = col,
                ButtonFunctionName = buttonFunctionName,
                ButtonFunctionText = buttonFunctionText

            };
            return htmlHelper.Partial("~/Views/Shared/Input/InputTextWithButton.cshtml", viewModel);
        }
        public static MvcHtmlString CustomTextBox(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6, bool required = false, int length = Int32.MaxValue,
            bool isReadonly = false, string cssClass = "k-input", object htmlAttribute = null,
            string placeHolderText = "", bool isPasswordType = false, bool isDisabled = false, string moreClass = "", string autoComplete = "on")
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);

            var viewModel = new InputTextViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                Length = length,
                ReadOnly = isReadonly,
                PlaceHolderText = placeHolderText,
                TextboxType = isPasswordType ? "type=password" : "type=text",
                Col = col,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                AutoComplete = autoComplete
            };
            if (isPasswordType)
            {
                //viewModel.Class += " k-textbox";
            }
            return htmlHelper.Partial("~/Views/Shared/Input/InputText.cshtml", viewModel);
        }

        public static MvcHtmlString CustomCheckBox(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6, bool isReadonly = false, string cssClass = "k-checkbox",
            object htmlAttribute = null, string ngChangeFunction = "", bool isDisabled = false, string moreClass = "", bool labelTop = false)
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);

            var viewModel = new CheckBoxViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                Col = col,
                NgChangeFunction = ngChangeFunction,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                LabelTop = labelTop
            };

            return htmlHelper.Partial("~/Views/Shared/Input/CheckBox.cshtml", viewModel);
        }

        public static MvcHtmlString CustomRadio(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6, bool isReadonly = false, string cssClass = "k-radio",
            object htmlAttribute = null, string ngChangeFunction = "", bool isDisabled = false,string value="", string moreClass = "")
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);

            var viewModel = new RadioViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                Col = col,
                NgChangeFunction = ngChangeFunction,
                IsDisabled = isDisabled,
                MoreClass = moreClass,
                Value = value
            };

            return htmlHelper.Partial("~/Views/Shared/Input/Radio.cshtml", viewModel);
        }

        public static MvcHtmlString CustomTextArea(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6, bool required = false, bool isReadonly = false,
            string cssClass = "k-textbox", object htmlAttribute = null, string placeHolderText = "", int cols = 2,
            int rows = 2, double widthPercentLable = 14, double widthPercentField = 80, int maxlength = Int32.MaxValue,
            int height = 100, string moreClass = "")
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);

            var viewModel = new AreaTextViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                PlaceHolderText = placeHolderText,
                Cols = cols,
                Rows = rows,
                WidthPercentLable = widthPercentLable,
                WidthPercentField = widthPercentField,
                MaxLength = maxlength,
                Col = col,
                Height = height,
                MoreClass = moreClass
            };

            return htmlHelper.Partial("~/Views/Shared/Input/TextArea.cshtml", viewModel);
        }

        public static MvcHtmlString CustomUploadFile(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6)
        {
            var viewModel = new FileUploadViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                Col = col
            };

            return htmlHelper.Partial("~/Views/Shared/Input/UploadFile.cshtml", viewModel);
        }

        public static MvcHtmlString CustomDatePicker(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col, string format,
            bool required = false, bool isReadonly = false, string cssClass = "k-datepicker",
            object htmlAttribute = null, DateTime? minDate = null, DateTime? maxDate = null)
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);
            var viewModel = new DatePickerViewModel
            {
                Id = id,
                Label = label,
                Format = format,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                MinDate = minDate,
                MaxDate = maxDate,
                Col = col
            };

            return htmlHelper.Partial("~/Views/Shared/Input/DatePicker.cshtml", viewModel);
        }

        public static MvcHtmlString CustomDatetimePicker(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col, string format,
            bool required = false, bool isReadonly = false, string cssClass = "k-datetimepicker",
            object htmlAttribute = null, bool hasTime = false, string placeHolderText = null, string moreClass = "")
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);

            var viewModel = new DateTimePickerViewModel
            {
                Id = id,
                Label = label,
                Format = format,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                Col = col,
                PlaceHolderText = placeHolderText,
                MoreClass = moreClass,
                HasTime = hasTime
            };

            return htmlHelper.Partial("~/Views/Shared/Input/DateTimePicker.cshtml", viewModel);
        }
        public static MvcHtmlString CustomTimePicker(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col, string format,
            bool required = false, bool isReadonly = false, string cssClass = "k-timepicker",
            object htmlAttribute = null, bool hasTime = false, bool hasMin = false, string moreClass = "")
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);

            var viewModel = new DateTimePickerViewModel
            {
                Id = id,
                Label = label,
                Format = format,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                Col = col,
                HasTime = hasTime,
                HasMin = hasMin,
                MoreClass = moreClass,
            };

            return htmlHelper.Partial("~/Views/Shared/Input/TimePicker.cshtml", viewModel);
        }

        public static MvcHtmlString CustomDatetimeRangePicker(this HtmlHelper htmlHelper,
            string idStart, string labelStart, string dataBindingValueStart,
            string idEnd, string labelEnd, string dataBindingValueEnd, int col, string format,
            bool hasTime = false, object htmlAttributeEnd = null, object htmlAttributeStart = null,
            bool required = false, bool isReadonly = false, string cssClass = "")
        {
            var attributeStart = new RouteValueDictionary();
            if (htmlAttributeStart != null)
            {
                attributeStart = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributeStart);
            }

            attributeStart.Add("id", idStart);

            var attributeEnd = new RouteValueDictionary();
            if (htmlAttributeEnd != null)
            {
                attributeEnd = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributeEnd);
            }

            attributeEnd.Add("id", idEnd);

            var viewModel = new DatetimeRangePickerViewModel
            {
                Class = cssClass,
                ReadOnly = isReadonly,
                Required = required,
                Format = format,

                IdStart = idStart,
                LabelStart = labelStart,
                HtmlAttributesStart = attributeStart,
                DataBindingValueStart = dataBindingValueStart,
                HasTime = hasTime,

                IdEnd = idEnd,
                LabelEnd = labelEnd,
                DataBindingValueEnd = dataBindingValueEnd,
                Col = col
            };

            return htmlHelper.Partial("~/Views/Shared/Input/DateTimeRangePicker.cshtml", viewModel);
        }

        private static MvcHtmlString InputNumericBase(this HtmlHelper htmlHelper, string id, string label, int col,
            int length = 10, string dataBindingValue = "", int width = 200,
            string format = "", object minimumValue = null,
            object maximumValue = null, double stepValue = 1, bool readOnly = false, int decimals = 2,
            string placeHolderText = "", bool isRequired = false)
        {
            var viewModel = new InputNumericViewModel
            {
                Id = id,
                Label = label,
                Length = length,
                DataBindingValue = string.IsNullOrEmpty(dataBindingValue) ? "''" : dataBindingValue,
                Width = width,
                Format = format,
                MinimumValue = minimumValue,
                MaximumValue = maximumValue,
                StepValue = stepValue,
                PlaceHolderText = placeHolderText,
                ReadOnly = readOnly,
                Decimals = decimals,
                Required = isRequired,
                Col = col

            };

            return htmlHelper.Partial("~/Views/Shared/Input/InputNumeric.cshtml", viewModel);
        }

        public static MvcHtmlString InputPositiveNumeric(this HtmlHelper htmlHelper, string id, string label,
                   string dataBindingValue, int col
                    , int width = 200, string format = "", int maximumValue = 999999999,
            bool readOnly = false, bool isRequired = false, int stepValue = 1)
        {
            var length = maximumValue.ToString(CultureInfo.InvariantCulture).Length;
            return InputNumericBase(htmlHelper, id, label, col, length, dataBindingValue, width, format,
                0, maximumValue, stepValue, readOnly, 0, "", isRequired);
        }

        public static MvcHtmlString InputNumeric(this HtmlHelper htmlHelper, string id, string label, int col, int length = 10,
            string dataBindingValue = "", int width = 200, string format = "",
            double minimumValue = 0.1,
            double maximumValue = 999999999, int stepValue = 1, bool readOnly = false, int decimals = 2,
            string placeHolderText = "", bool isRequired = false)
        {
            var decimalLength = decimals > 0 ? decimals + 1 : 0;
            var maxlength = maximumValue.ToString(CultureInfo.InvariantCulture).Length + decimalLength;
            if (decimals > 0 && string.IsNullOrEmpty(format))
            {
                format = string.Format("#,#.{0}", "0".PadRight(decimals, '0'));
            }
            return InputNumericBase(htmlHelper, id, label, col, maxlength, dataBindingValue, width,
                format, minimumValue,
                maximumValue, stepValue, readOnly, decimals, placeHolderText, isRequired);
        }

        public static MvcHtmlString CaymanZipcode(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col, bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null)
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, col, "00000", isRequired,
                readOnly,
                htmlAttribute);
        }

        public static MvcHtmlString CustomPhone(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col, bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null)
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, col, "(999) 000-0000", isRequired,
                readOnly,
                htmlAttribute);
        }

        public static MvcHtmlString CustomSocialSecurityNumber(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6, bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null, string placeHolderText = null, string moreClass = "")
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, col, "000-00-0000", isRequired,
                readOnly,
                htmlAttribute, placeHolderText, moreClass);
        }

        public static MvcHtmlString CustomNPI(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6, bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null)
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, col, "0000000000", isRequired,
                readOnly,
                htmlAttribute);
        }

        public static MvcHtmlString CustomReqNumPrefix(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6, bool isRequired = false, bool readOnly = false,
            object htmlAttribute = null)
        {
            return InputMasked(htmlHelper, id, label, dataBindingValue, col, "LLL", isRequired,
                readOnly,
                htmlAttribute);
        }


        public static MvcHtmlString InputMasked(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col, string format, bool isRequired = false,
            bool readOnly = false,
            object htmlAttribute = null, string placeHolderText = null, string moreClass = "")
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);

            var viewModel = new InputMaskedViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = isRequired,
                DataBindingValue = dataBindingValue,
                Format = format,
                ReadOnly = readOnly,
                Col = col,
                PlaceHolderText = placeHolderText,
                MoreClass = moreClass
            };

            return htmlHelper.Partial("~/Views/Shared/Input/InputMasked.cshtml", viewModel);
        }

        public static MvcHtmlString CustomEditor(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col, bool required = false, int width = 50, int height = 50,
            string urlRead = null,
            string urlDestroy = null, string urlCreate = null, string urlThumb = null, string urlUpload = null,
            string urlImage = null, bool isReadonly = false, string cssClass = "k-textbox", object htmlAttribute = null)
        {
            var attribute = new RouteValueDictionary();
            if (htmlAttribute != null)
            {
                attribute = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            }

            attribute.Add("id", id);

            var viewModel = new EditorViewModel
            {
                Id = id,
                Label = label,
                HtmlAttributes = attribute,
                Required = required,
                Class = cssClass,
                DataBindingValue = dataBindingValue,
                ReadOnly = isReadonly,
                Width = width,
                Height = height,
                UrlRead = urlRead,
                UrlDestroy = urlDestroy,
                UrlCreate = urlCreate,
                UrlThumb = urlThumb,
                UrlUpload = urlUpload,
                UrlImage = urlImage,
                Col = col
            };

            return htmlHelper.Partial("~/Views/Shared/Input/Editor.cshtml", viewModel);
        }

        public static MvcHtmlString InputWithAttributes(this HtmlHelper helper, string id, object htmlAttributes = null)
        {
            var control = new TagBuilder("input");

            foreach (var attribute in (RouteValueDictionary)htmlAttributes)
                control.MergeAttribute(attribute.Key, attribute.Value.ToString());

            return new MvcHtmlString(control.ToString());
        }

        public static MvcHtmlString CustomDropDownList(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col = 6, string readUrl = "", bool required = false, object currentValue = null,
            bool readOnly = false, string eventChange = "", string moreClass = "")
        {
            var viewModel = new DropDownListViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                Col = col,
                Required = required,
                CurrentValue = currentValue,
                ReadUrl = readUrl,
                ReadOnly = readOnly,
                EventChange = eventChange,
                MoreClass = moreClass
            };
            return htmlHelper.Partial("~/Views/Shared/Input/DropDownList.cshtml", viewModel);
        }


        public static MvcHtmlString CustomFileUpload(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col, string saveUrl = "/Common/SaveFileUpload", string removeUrl = "/Common/RemoveFileUpload",
            int previewWidth = 32, int previewHeight = 32, bool required = false, string acceptType = "*", bool isAvatar=false)
        {
            var viewModel = new FileUploadViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                Col = col,
                Required = required,
                SaveUrl = saveUrl,
                RemoveUrl = removeUrl,
                PreviewHeight = previewHeight,
                PreviewWidth = previewWidth,
                AcceptType = acceptType,
                IsAvatar = isAvatar
            };
            return htmlHelper.Partial("~/Views/Shared/Input/FileUpload.cshtml", viewModel);
        }

        public static MvcHtmlString CustomFileUploadAngular(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, bool isMultiple = false, int col = 5, string saveUrl = "/Common/SaveFileUpload", string removeUrl = "/Common/RemoveFileUpload",
            int previewWidth = 32, int previewHeight = 32, bool required = false, string acceptType = "*")
        {
            var viewModel = new FileUploadViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                Col = col,
                Required = required,
                SaveUrl = saveUrl,
                RemoveUrl = removeUrl,
                PreviewHeight = previewHeight,
                PreviewWidth = previewWidth,
                AcceptType = acceptType,
                IsMultiple = isMultiple,
            };
            return htmlHelper.Partial("~/Views/Shared/Input/FileUploadAngular.cshtml", viewModel);
        }

        public static MvcHtmlString CustomFileUploadPatient(this HtmlHelper htmlHelper, string id, string label,
            string dataBindingValue, int col, string saveUrl, string removeUrl,
            int previewWidth, int previewHeight, bool required = false)
        {
            var viewModel = new FileUploadViewModel
            {
                Id = id,
                Label = label,
                DataBindingValue = dataBindingValue,
                Col = col,
                Required = required,
                SaveUrl = saveUrl,
                RemoveUrl = removeUrl,
                PreviewHeight = previewHeight,
                PreviewWidth = previewWidth
            };
            return htmlHelper.Partial("~/Views/Shared/Input/FileUploadPatient.cshtml", viewModel);
        }
        
    }
}